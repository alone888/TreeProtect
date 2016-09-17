#include "sys.h"
#include "usart.h"
#include "crc16.h"	  
#include "string.h"	
#include "calib.h"
#include "stdlib.h"	
#include "LED.h"	   
//********************************************************************************
//V1.3修改说明 
//支持适应不同频率下的串口波特率设置.
//加入了对printf的支持
//增加了串口接收命令功能.
//修正了printf第一个字符丢失的bug
//V1.4修改说明
//1,修改串口初始化IO的bug
//2,修改了USART_RX_STA,使得串口最大接收字节数为2的14次方
//3,增加了USART_REC_LEN,用于定义串口最大允许接收的字节数(不大于2的14次方)
//4,修改了EN_USART1_RX的使能方式
//V1.5修改说明
//1,增加了对UCOSII的支持
////////////////////////////////////////////////////////////////////////////////// 	  
 

//////////////////////////////////////////////////////////////////
//加入以下代码,支持printf函数,而不需要选择use MicroLIB	  
#if 1
#pragma import(__use_no_semihosting)             
//标准库需要的支持函数                 
struct __FILE 
{ 
	int handle; 

}; 

FILE __stdout;       
//定义_sys_exit()以避免使用半主机模式    
_sys_exit(int x) 
{ 
	x = x; 
} 
//重定义fputc函数 
int fputc(int ch, FILE *f)
{      
 	while(USART_GetFlagStatus(USART1,USART_FLAG_TC)==RESET); 
   USART_SendData(USART1,(uint8_t)ch);   
	
// 	while(USART_GetFlagStatus(USART2,USART_FLAG_TC)==RESET); 
//   USART_SendData(USART2,(uint8_t)ch);  
 	
// 	while(USART_GetFlagStatus(USART3,USART_FLAG_TC)==RESET); 
//   USART_SendData(USART3,(uint8_t)ch);  
 	
// 	while(USART_GetFlagStatus(UART4,USART_FLAG_TC)==RESET); 
//   USART_SendData(UART4,(uint8_t)ch);  
 	
//	while(USART_GetFlagStatus(UART5,USART_FLAG_TC)==RESET); 
//  USART_SendData(UART5,(uint8_t)ch);  

	return ch;
}
#endif 

/*使用microLib的方法*/
 /* 
int fputc(int ch, FILE *f)
{
	USART_SendData(USART1, (uint8_t) ch);

	while (USART_GetFlagStatus(USART1, USART_FLAG_TC) == RESET) {}	
   
    return ch;
}
int GetKey (void)  { 

    while (!(USART1->SR & USART_FLAG_RXNE));

    return ((int)(USART1->DR & 0x1FF));
}
*/
 
#if EN_USART1_RX   //如果使能了接收


//串口1中断服务程序
//注意,读取USARTx->SR能避免莫名其妙的错误   	
char USART_RX_BUF[5][USART_REC_LEN];     //接收缓冲,最大USART_REC_LEN个字节.



//接收状态
//bit15，	接收完成标志
//bit14，	接收到0x0d
//bit13~0，	接收到的有效字节数目
u16 USART_RX_STA=0;       //接收状态标记


extern short g_IndVal[4];//±5mm  ±5000  以um为单位
extern int g_Pa; //先以电压表示 不标定。
extern short g_WindSpeed;
extern short g_WindDir;
extern int g_Temper;
extern char g_weightCalib[4];


extern unsigned char F_wait_response;
extern unsigned char SD_cmd_return_byte;


	  

//初始化IO 串口1 
//bound:波特率
void uart1_init(u32 bound){
	//GPIO端口设置
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_USART1|RCC_APB2Periph_GPIOA, ENABLE);	//使能USART1，GPIOA时钟
	USART_DeInit(USART1);  //复位串口1
	//USART1_TX   PA.9
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9; //PA.9
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
	GPIO_Init(GPIOA, &GPIO_InitStructure); //初始化PA9
	
	//USART1_RX	  PA.10
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
	GPIO_Init(GPIOA, &GPIO_InitStructure);  //初始化PA10
	
	
	//USART 初始化设置
	
	USART_InitStructure.USART_BaudRate = bound;//一般设置为9600;
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//字长为8位数据格式
	USART_InitStructure.USART_StopBits = USART_StopBits_1;//一个停止位
	USART_InitStructure.USART_Parity = USART_Parity_No;//无奇偶校验位
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//无硬件数据流控制
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//收发模式
	
	USART_Init(USART1, &USART_InitStructure); //初始化串口
	#if EN_USART1_RX		  //如果使能了接收  
		//Usart1 NVIC 配置
		NVIC_InitStructure.NVIC_IRQChannel = USART1_IRQn;
		NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//抢占优先级3
		NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//子优先级3
		NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQ通道使能
		NVIC_Init(&NVIC_InitStructure);	//根据指定的参数初始化VIC寄存器
		
		USART_ITConfig(USART1, USART_IT_RXNE, ENABLE);//开启中断
	#endif
	USART_Cmd(USART1, ENABLE);                    //使能串口 

}

 void uart2_init(u32 bound){
     //GPIO端口设置
     GPIO_InitTypeDef GPIO_InitStructure;
 	USART_InitTypeDef USART_InitStructure;
 	NVIC_InitTypeDef NVIC_InitStructure;
 	 
 	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART2,ENABLE);	//使能USART2时钟
 	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOA,ENABLE);	//使能GPIOA时钟
  	USART_DeInit(USART2);  //复位串口2
 	 //USART2_TX   PA.2
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_2; //PA.2
     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
     GPIO_Init(GPIOA, &GPIO_InitStructure); //初始化PA2
    
     //USART2_RX	  PA.3
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_3;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
     GPIO_Init(GPIOA, &GPIO_InitStructure);  //初始化PA3


    //USART 初始化设置

 	USART_InitStructure.USART_BaudRate = bound;//一般设置为9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//字长为8位数据格式
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//一个停止位
 	USART_InitStructure.USART_Parity = USART_Parity_No;//无奇偶校验位
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//无硬件数据流控制
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//收发模式

     USART_Init(USART2, &USART_InitStructure); //初始化串口
 #if EN_USART2_RX		  //如果使能了接收  
    //Usart2 NVIC 配置
     NVIC_InitStructure.NVIC_IRQChannel = USART2_IRQn;
 	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//抢占优先级3
 	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//子优先级3
 	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQ通道使能
 	NVIC_Init(&NVIC_InitStructure);	//根据指定的参数初始化VIC寄存器
    
     USART_ITConfig(USART2, USART_IT_RXNE, ENABLE);//开启中断
 #endif
     USART_Cmd(USART2, ENABLE);                    //使能串口 

 }

 void uart3_init(u32 bound){
	//GPIO端口设置
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3, ENABLE);	//使能USART3时钟

//	//无重映射
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB, ENABLE);   
//	
//	//部分重映射
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC, ENABLE);    
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//使能USART3时钟
//	GPIO_PinRemapConfig(GPIO_PartialRemap_USART3,ENABLE);
	
	//完全重映射
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOD, ENABLE);    
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//使能USART3时钟
	GPIO_PinRemapConfig(GPIO_FullRemap_USART3,ENABLE);
	
	USART_DeInit(USART3);  //复位串口3
 	
 	/*USART3没有重映射  */
 	 //USART3_TX   PB.10
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
     GPIO_Init(GPIOB, &GPIO_InitStructure); //初始化PB10
    
     //USART3_RX	  PB.11
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
     GPIO_Init(GPIOB, &GPIO_InitStructure);  //初始化PB11

 	
 	/*部分重映射*/
 // 	 //USART3_TX   PC.10
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
 //     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
 //     GPIO_Init(GPIOC, &GPIO_InitStructure); //初始化PC10
 //    
 //     //USART3_RX	  PC.11
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
 //     GPIO_Init(GPIOC, &GPIO_InitStructure);  //初始化PC11
 	
 	/*完全重映射*/
	//USART3_TX   PD.8
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8; //PD.8
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
	GPIO_Init(GPIOD, &GPIO_InitStructure); //初始化PD8
	
	//USART3_RX	  PD.9
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
	GPIO_Init(GPIOD, &GPIO_InitStructure);  //初始化PD9

    //USART 初始化设置

 	USART_InitStructure.USART_BaudRate = bound;//一般设置为9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//字长为8位数据格式
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//一个停止位
 	USART_InitStructure.USART_Parity = USART_Parity_No;//无奇偶校验位
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//无硬件数据流控制
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//收发模式

     USART_Init(USART3, &USART_InitStructure); //初始化串口
 #if EN_USART3_RX		  //如果使能了接收  
    //Usart3 NVIC 配置
	NVIC_InitStructure.NVIC_IRQChannel = USART3_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//抢占优先级3
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//子优先级3
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQ通道使能
	NVIC_Init(&NVIC_InitStructure);	//根据指定的参数初始化VIC寄存器
	
	USART_ITConfig(USART3, USART_IT_RXNE, ENABLE);//开启中断
 #endif
     USART_Cmd(USART3, ENABLE);                    //使能串口 

 }
 //没有重映射
void uart3_init2(u32 bound){
	//GPIO端口设置
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3, ENABLE);	//使能USART3时钟
	
	  
	//无重映射  一定要关重映射啊！！！！
	GPIO_PinRemapConfig(GPIO_FullRemap_USART3,DISABLE);
	GPIO_PinRemapConfig(GPIO_PartialRemap_USART3,DISABLE); 
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB, ENABLE);


	
//	//部分重映射
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC, ENABLE);    
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//使能USART3时钟
//	GPIO_PinRemapConfig(GPIO_PartialRemap_USART3,ENABLE);
//	
//	//完全重映射
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOD, ENABLE);    
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//使能USART3时钟
//	GPIO_PinRemapConfig(GPIO_FullRemap_USART3,ENABLE);
	
	USART_DeInit(USART3);  //复位串口3
	
	/*USART3没有重映射  */
	//USART3_TX   PB.10
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
	GPIO_Init(GPIOB, &GPIO_InitStructure); //初始化PB10
	
	//USART3_RX	  PB.11
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
	GPIO_Init(GPIOB, &GPIO_InitStructure);  //初始化PB11

 	
 	/*部分重映射*/
 // 	 //USART3_TX   PC.10
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
 //     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
 //     GPIO_Init(GPIOC, &GPIO_InitStructure); //初始化PC10
 //    
 //     //USART3_RX	  PC.11
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
 //     GPIO_Init(GPIOC, &GPIO_InitStructure);  //初始化PC11
 	
 	/*完全重映射*/
//	//USART3_TX   PD.8
//	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8; //PD.8
//	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
//	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
//	GPIO_Init(GPIOD, &GPIO_InitStructure); //初始化PD8
//	
//	//USART3_RX	  PD.9
//	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;
//	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
//	GPIO_Init(GPIOD, &GPIO_InitStructure);  //初始化PD9

    //USART 初始化设置

 	USART_InitStructure.USART_BaudRate = bound;//一般设置为9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//字长为8位数据格式
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//一个停止位
 	USART_InitStructure.USART_Parity = USART_Parity_No;//无奇偶校验位
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//无硬件数据流控制
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//收发模式

     USART_Init(USART3, &USART_InitStructure); //初始化串口
 #if EN_USART3_RX		  //如果使能了接收  
    //Usart3 NVIC 配置
	NVIC_InitStructure.NVIC_IRQChannel = USART3_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//抢占优先级3
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//子优先级3
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQ通道使能
	NVIC_Init(&NVIC_InitStructure);	//根据指定的参数初始化VIC寄存器
	
	USART_ITConfig(USART3, USART_IT_RXNE, ENABLE);//开启中断
 #endif
     USART_Cmd(USART3, ENABLE);                    //使能串口 

 }

 void uart4_init(u32 bound){
     //GPIO端口设置
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC, ENABLE);	//使能GPIOC时钟
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_UART4,ENABLE);
	USART_DeInit(UART4);  //复位串口4
	//USART4_TX   PC.10
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PC.10
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
	GPIO_Init(GPIOC, &GPIO_InitStructure); //初始化PC10
	
	//USART4_RX	  PC.11
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
	GPIO_Init(GPIOC, &GPIO_InitStructure);  //初始化PC11


    //USART 初始化设置

 	USART_InitStructure.USART_BaudRate = bound;//一般设置为9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//字长为8位数据格式
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//一个停止位
 	USART_InitStructure.USART_Parity = USART_Parity_No;//无奇偶校验位
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//无硬件数据流控制
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//收发模式

     USART_Init(UART4, &USART_InitStructure); //初始化串口
 #if EN_USART4_RX		  //如果使能了接收  
    //Usart4 NVIC 配置
     NVIC_InitStructure.NVIC_IRQChannel = UART4_IRQn;
 	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//抢占优先级3
 	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//子优先级3
 	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQ通道使能
 	NVIC_Init(&NVIC_InitStructure);	//根据指定的参数初始化VIC寄存器
    
     USART_ITConfig(UART4, USART_IT_RXNE, ENABLE);//开启中断
 #endif
     USART_Cmd(UART4, ENABLE);                    //使能串口 

 }

void uart5_init(u32 bound){
    //GPIO端口设置
    GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	 
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_UART5, ENABLE);	//使能GPIOA时钟
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOD|RCC_APB2Periph_GPIOC, ENABLE);
 	USART_DeInit(UART5);  //复位串口1
	 //USART5_TX   PC12
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_12; //PC.12
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//复用推挽输出
    GPIO_Init(GPIOC, &GPIO_InitStructure); //初始化PC.12
   
    //USART5_RX	  PD.2
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_2;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//浮空输入
    GPIO_Init(GPIOD, &GPIO_InitStructure);  //初始化PD2


   //USART 初始化设置

	USART_InitStructure.USART_BaudRate = bound;//一般设置为9600;
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//字长为8位数据格式
	USART_InitStructure.USART_StopBits = USART_StopBits_1;//一个停止位
	USART_InitStructure.USART_Parity = USART_Parity_No;//无奇偶校验位
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//无硬件数据流控制
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//收发模式

    USART_Init(UART5, &USART_InitStructure); //初始化串口
#if EN_USART5_RX		  //如果使能了接收  
   //Usart5 NVIC 配置
    NVIC_InitStructure.NVIC_IRQChannel = UART5_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//抢占优先级3
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//子优先级3
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQ通道使能
	NVIC_Init(&NVIC_InitStructure);	//根据指定的参数初始化VIC寄存器
   
    USART_ITConfig(UART5, USART_IT_RXNE, ENABLE);//开启中断
#endif
    USART_Cmd(UART5, ENABLE);                    //使能串口 

}

int send_byte_to_usart1(char data)
{
	USART_SendData(USART1, data);//向串口5发送数据
	while(USART_GetFlagStatus(USART1,USART_FLAG_TC)!=SET);//等待发送结束
	return 0;
}
					
int send_byte_to_usart2(char data)
{
	USART_SendData(USART2, data);//向串口5发送数据
	while(USART_GetFlagStatus(USART2,USART_FLAG_TC)!=SET);//等待发送结束
	return 0;
}

int send_byte_to_usart3(char data)
{
	USART_SendData(USART3, data);//向串口5发送数据
	while(USART_GetFlagStatus(USART3,USART_FLAG_TC)!=SET);//等待发送结束
	return 0;
}

int send_byte_to_usart4(char data)
{
	USART_SendData(UART4, data);//向串口5发送数据
	while(USART_GetFlagStatus(UART4,USART_FLAG_TC)!=SET);//等待发送结束
	return 0;
}	

int send_byte_to_usart5(char data)
{
	USART_SendData(UART5, data);//向串口5发送数据
	while(USART_GetFlagStatus(UART5,USART_FLAG_TC)!=SET);//等待发送结束
	return 0;
}

/**********************************************************************/
//   以下为发送字符串
/**********************************************************************/
int send_string_to_usart1(char * str)
{
	while (*str != '\0') {
		send_byte_to_usart1(*str++);
	}  
	return 0;
}

int send_string_to_usart2(char * str)
{	 
	while (*str != '\0') {
		send_byte_to_usart2(*str++);
	}  
	return 0;
}
int send_string_to_usart3(char * str)
{	 
	while (*str != '\0') {
		send_byte_to_usart3(*str++);
	}
	return 0;
}

int send_string_to_usart4(char * str)
{	 
	while (*str != '\0') {
		send_byte_to_usart4(*str++);
	}
	return 0;
}

int send_string_to_usart5(char * str)
{	 
	while (*str != '\0') {
		send_byte_to_usart2(*str++);
	}
	return 0;
}

/**********************************************************************/
//   以下为发送数组
/**********************************************************************/
int send_buf_to_usart1(unsigned char * str, unsigned int buflen)
{
	int i=0;
	for(i=0;i<buflen;i++) {
		send_byte_to_usart1(*str++);
	}  
	return 0;
}

int send_buf_to_usart2(unsigned char * str, unsigned int buflen)
{	 
	int i=0;
	for(i=0;i<buflen;i++)  {
		send_byte_to_usart2(*str++);
	}  
	return 0;
}
int send_buf_to_usart3(unsigned char * str, unsigned int buflen)
{	 
	int i=0;
	for(i=0;i<buflen;i++) {
		send_byte_to_usart3(*str++);
	}
	return 0;
}

int send_buf_to_usart4(unsigned char * str, unsigned int buflen)
{	 
	int i=0;
	for(i=0;i<buflen;i++) {
		send_byte_to_usart4(*str++);
	}
	return 0;
}

int send_buf_to_usart5(unsigned char * str, unsigned int buflen)
{	 
	int i=0;
	for(i=0;i<buflen;i++) {
		send_byte_to_usart2(*str++);
	}
	return 0;
}



char flagstart;
char rxCnt;
u8 IndId,CalibCnt;
char PC_cmd;//01 为查询状态  10-14 读取标定数据	20-24写入标定数据
char PC_Wcmd;
u16 load;

/*******************************************************************************/	  
//
//								与上位机通讯
/*******************************************************************************/

void USART1_IRQHandler(void)                	//串口1中断服务程序
{
	u8 Res;
	char* token;
	
	if(USART_GetITStatus(USART1, USART_IT_RXNE) != RESET)  //接收中断
	{
		Res =USART_ReceiveData(USART1);//(USART1->DR);	//读取接收到的数据

		if(flagstart)
		{
			USART_RX_BUF[0][rxCnt++] = Res;
		}
		
		if('@'==Res)
		{
			flagstart = 1;
			memset(USART_RX_BUF[0],0,USART_REC_LEN);
			rxCnt = 0;		
		}
		if('!'==Res && 1 == flagstart )
		{
			flagstart = 0;
			if(USART_RX_BUF[0][0]=='0' && USART_RX_BUF[0][1]=='1' && USART_RX_BUF[0][2]==',' )
			{
				PC_cmd = 1;	
			}
			if(USART_RX_BUF[0][0]=='1'                            && USART_RX_BUF[0][2]==',' )
			{
				PC_cmd = 0x10 + USART_RX_BUF[0][1]-'0';	
			}
			if(USART_RX_BUF[0][0]=='2'                            && USART_RX_BUF[0][2]==',' )
			{
				BEEP = 1;
				
				IndId = USART_RX_BUF[0][1]-'0';
				CalibCnt = 0;
				token = strtok( USART_RX_BUF[0] , ",");
				token = strtok( NULL, ","); 
				while( token != NULL )
			    {
			        /* While there are tokens in "string" */
					if(CalibCnt > 20)
					{
						break;
					}
			        IndCalib[IndId][CalibCnt++] = atoi(token);
			        /* Get next token: */
			        token = strtok( NULL, ",");
			    }
				Flash_Write(0x08042000,(u8 *)IndCalib,sizeof(IndCalib));
				Flash_Read(0x08042000,(u8 *)IndCalib,sizeof(IndCalib));
				PC_cmd = 0x20 + USART_RX_BUF[0][1]-'0';
				
				BEEP = 0;	
			}
			//标定压力0位
			if(USART_RX_BUF[0][0]=='3'                            && USART_RX_BUF[0][2]==',' )
			{

				PC_Wcmd = 0x30; 	
			}
			//标定压力载重
			if(USART_RX_BUF[0][0]=='4'                            && USART_RX_BUF[0][2]==',' )
			{
				token = strtok( USART_RX_BUF[0] , ",");
				token = strtok( NULL, ",");
 				load = atoi(token);
				g_weightCalib[0]= load >>8;
				g_weightCalib[1]= (load <<8)>>8;				
				PC_Wcmd = 0x40; 	
			}


		}	 
	}
} 
/*******************************************************************************/	  
//
//								查询风速
/*******************************************************************************/
void USART2_IRQHandler(void)                	//串口2中断服务程序  SD卡 接收
{
	//接收风速
	int i;
	if(USART_GetITStatus(USART2, USART_IT_RXNE) != RESET)  //接收中断
	{
		USART_RX_BUF[1][12] =USART_ReceiveData(USART2);	//读取接收到的数据
		
		//顺序往前推
		for(i=0;i<12;i++)
		{
			USART_RX_BUF[1][i] = USART_RX_BUF[1][i+1];	
		}

		//地址1 是风速
		if(USART_RX_BUF[1][0] == 0x01 && USART_RX_BUF[1][1] == 0x03 && USART_RX_BUF[1][2] == 0x02)
		//if(USART_RX_BUF[1][0] == 0x00 && USART_RX_BUF[1][1] == 0x10 && USART_RX_BUF[1][2] == 0x01)
		{
			int crc16 = 0;
			char crcFirst=0,crcAfter = 0;
			crc16 = getCRC16_LowFirst(USART_RX_BUF[1],5);
			crcFirst = (crc16 & (0xFF<<8))>>8;
			crcAfter = 	crc16;
			//printf("U2-2\r\n");
			if(USART_RX_BUF[1][5] == crcFirst && USART_RX_BUF[1][6] == crcAfter)//CRC通过
			{		
				g_WindSpeed = USART_RX_BUF[1][3];
				g_WindSpeed = (g_WindSpeed<<8) + USART_RX_BUF[1][4];
				g_Temper = DS18B20_Get_Temp(); //转一下吧
				//printf("U2-3\r\n");
			}
		}
	}
 }
 
/*******************************************************************************/	  
//
//								读写SD卡 查询风向取消
/*******************************************************************************/ 
extern char g_uart3_used_for_SD;
void USART3_IRQHandler(void)                	//串口4中断服务程序
{
	char temp;
	int i;
	if(USART_GetITStatus(USART3, USART_IT_RXNE) != RESET)  //接收中断
	{

		temp =USART_ReceiveData(USART3);
		if(F_wait_response == 0x01)
		{
			SD_cmd_return_byte = temp;
			F_wait_response = 0;
		}
	}	
} 
//称重
void UART4_IRQHandler(void)                	//串口4中断服务程序
{
	int i;
	int Pa;
	char weight[4];
	if(USART_GetITStatus(UART4, USART_IT_RXNE) != RESET)  //接收中断
	{
		USART_RX_BUF[3][USART_REC_LEN-1] =USART_ReceiveData(UART4);	//读取接收到的数据
		
		//顺序往前推
		for(i=0;i<USART_REC_LEN-1;i++)
		{
			USART_RX_BUF[3][i] = USART_RX_BUF[3][i+1];	
		}
		if(USART_RX_BUF[3][0] == 0x01 && USART_RX_BUF[3][1] == 0x03 && USART_RX_BUF[3][2] == 0x04)
		{
			int crc16 = 0;
			char crcFirst=0,crcAfter = 0;
			crc16 = getCRC16_LowFirst(USART_RX_BUF[3],7);
			crcFirst = (crc16 & (0xFF<<8))>>8;
			crcAfter = 	crc16;
			if(USART_RX_BUF[3][7] == crcFirst && USART_RX_BUF[3][8] == crcAfter)//CRC通过
			{		
				for(i=0;i<4;i++)
				{
					weight[i] = USART_RX_BUF[3][3+i];
				}
					    
				Pa = 0;
				Pa = weight[2];
				Pa = (Pa<<8) + weight[3];
				Pa = (Pa<<8) + weight[0];
				Pa = (Pa<<8) + weight[1];
				g_Pa = -Pa;

				//printf("U4-2\r\n");
			}
		}
		//零点标定
		if(USART_RX_BUF[3][0] == 0x01 && USART_RX_BUF[3][1] == 0x06 
			&& USART_RX_BUF[3][2] == 0x00 && USART_RX_BUF[3][3] == 0x12
			&& USART_RX_BUF[3][4] == 0x00 && USART_RX_BUF[3][5] == 0x01
			&& USART_RX_BUF[3][6] == 0xE8 && USART_RX_BUF[3][7] == 0x0F)
		{
		}
		//负载标定1
		if(USART_RX_BUF[3][0] == 0x01 && USART_RX_BUF[3][1] == 0x06 
			&& USART_RX_BUF[3][2] == 0x00 && USART_RX_BUF[3][3] == 0x12
			&& USART_RX_BUF[3][4] == 0x00 && USART_RX_BUF[3][5] == 0x02
			&& USART_RX_BUF[3][6] == 0xA8 && USART_RX_BUF[3][7] == 0x0E)
		{
			PC_Wcmd = 0x41;	
		}
		//负载标定2
		if(USART_RX_BUF[3][0] == 0x01 && USART_RX_BUF[3][1] == 0x10 
			&& USART_RX_BUF[3][2] == 0x00 && USART_RX_BUF[3][3] == 0x08
			&& USART_RX_BUF[3][4] == 0x00 && USART_RX_BUF[3][5] == 0x02)
		{
		}
	}
}  	

/*******************************************************************************/	  
//                           读取电感
//
//                 |起始寄存器|	4个寄存器 4路AD| crc校验 低位在前|
// 读AD 发送 01 03   00 68       00 04			  44 03
//              |0x30 为第9路AD| 

//                | 收到8字节 |	第9路 |第10路|第11路|第12路| 校验
// 收到      01 03      08 	    XX XX  XX XX  XX XX  XX XX

/*******************************************************************************/
void UART5_IRQHandler(void)                	//串口5中断服务程序
{
	int i;
	char ind[8];
	if(USART_GetITStatus(UART5, USART_IT_RXNE) != RESET)  //接收中断
	{
		USART_RX_BUF[4][USART_REC_LEN-1] =USART_ReceiveData(UART5);//(USART1->DR);	//读取接收到的数据
		
		//顺序往前推
		for(i=0;i<USART_REC_LEN-1;i++)
		{
			USART_RX_BUF[4][i] = USART_RX_BUF[4][i+1];	
		}

		if(USART_RX_BUF[4][0] == 0x01 && USART_RX_BUF[4][1] == 0x03 && USART_RX_BUF[4][2] == 0x08)
		{
			int crc16 = 0;
			char crcFirst=0,crcAfter = 0;
			crc16 = getCRC16_LowFirst(USART_RX_BUF[4],11);
			crcFirst = (crc16 & (0xFF<<8))>>8;
			crcAfter = 	crc16;
			if(USART_RX_BUF[4][11] == crcFirst && USART_RX_BUF[4][12] == crcAfter)//CRC通过
			{		
				for(i=0;i<8;i++)
				{
					ind[i] = USART_RX_BUF[4][3+i];
				}
				for(i=0;i<4;i++)
				{
					g_IndVal[i] = 0;
					g_IndVal[i] = ind[2*i];
					g_IndVal[i] = g_IndVal[i]<< 8;
					g_IndVal[i] |= ind[2*i+1];
					g_Temper = DS18B20_Get_Temp();
				}
			}
		}
	}
} 
#endif	

