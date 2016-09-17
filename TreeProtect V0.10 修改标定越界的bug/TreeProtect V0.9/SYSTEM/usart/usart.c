#include "sys.h"
#include "usart.h"
#include "crc16.h"	  
#include "string.h"	
#include "calib.h"
#include "stdlib.h"	
#include "LED.h"	   
//********************************************************************************
//V1.3�޸�˵�� 
//֧����Ӧ��ͬƵ���µĴ��ڲ���������.
//�����˶�printf��֧��
//�����˴��ڽ��������.
//������printf��һ���ַ���ʧ��bug
//V1.4�޸�˵��
//1,�޸Ĵ��ڳ�ʼ��IO��bug
//2,�޸���USART_RX_STA,ʹ�ô����������ֽ���Ϊ2��14�η�
//3,������USART_REC_LEN,���ڶ��崮�����������յ��ֽ���(������2��14�η�)
//4,�޸���EN_USART1_RX��ʹ�ܷ�ʽ
//V1.5�޸�˵��
//1,�����˶�UCOSII��֧��
////////////////////////////////////////////////////////////////////////////////// 	  
 

//////////////////////////////////////////////////////////////////
//�������´���,֧��printf����,������Ҫѡ��use MicroLIB	  
#if 1
#pragma import(__use_no_semihosting)             
//��׼����Ҫ��֧�ֺ���                 
struct __FILE 
{ 
	int handle; 

}; 

FILE __stdout;       
//����_sys_exit()�Ա���ʹ�ð�����ģʽ    
_sys_exit(int x) 
{ 
	x = x; 
} 
//�ض���fputc���� 
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

/*ʹ��microLib�ķ���*/
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
 
#if EN_USART1_RX   //���ʹ���˽���


//����1�жϷ������
//ע��,��ȡUSARTx->SR�ܱ���Ī������Ĵ���   	
char USART_RX_BUF[5][USART_REC_LEN];     //���ջ���,���USART_REC_LEN���ֽ�.



//����״̬
//bit15��	������ɱ�־
//bit14��	���յ�0x0d
//bit13~0��	���յ�����Ч�ֽ���Ŀ
u16 USART_RX_STA=0;       //����״̬���


extern short g_IndVal[4];//��5mm  ��5000  ��umΪ��λ
extern int g_Pa; //���Ե�ѹ��ʾ ���궨��
extern short g_WindSpeed;
extern short g_WindDir;
extern int g_Temper;
extern char g_weightCalib[4];


extern unsigned char F_wait_response;
extern unsigned char SD_cmd_return_byte;


	  

//��ʼ��IO ����1 
//bound:������
void uart1_init(u32 bound){
	//GPIO�˿�����
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_USART1|RCC_APB2Periph_GPIOA, ENABLE);	//ʹ��USART1��GPIOAʱ��
	USART_DeInit(USART1);  //��λ����1
	//USART1_TX   PA.9
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9; //PA.9
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
	GPIO_Init(GPIOA, &GPIO_InitStructure); //��ʼ��PA9
	
	//USART1_RX	  PA.10
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
	GPIO_Init(GPIOA, &GPIO_InitStructure);  //��ʼ��PA10
	
	
	//USART ��ʼ������
	
	USART_InitStructure.USART_BaudRate = bound;//һ������Ϊ9600;
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//�ֳ�Ϊ8λ���ݸ�ʽ
	USART_InitStructure.USART_StopBits = USART_StopBits_1;//һ��ֹͣλ
	USART_InitStructure.USART_Parity = USART_Parity_No;//����żУ��λ
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//��Ӳ������������
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//�շ�ģʽ
	
	USART_Init(USART1, &USART_InitStructure); //��ʼ������
	#if EN_USART1_RX		  //���ʹ���˽���  
		//Usart1 NVIC ����
		NVIC_InitStructure.NVIC_IRQChannel = USART1_IRQn;
		NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//��ռ���ȼ�3
		NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//�����ȼ�3
		NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQͨ��ʹ��
		NVIC_Init(&NVIC_InitStructure);	//����ָ���Ĳ�����ʼ��VIC�Ĵ���
		
		USART_ITConfig(USART1, USART_IT_RXNE, ENABLE);//�����ж�
	#endif
	USART_Cmd(USART1, ENABLE);                    //ʹ�ܴ��� 

}

 void uart2_init(u32 bound){
     //GPIO�˿�����
     GPIO_InitTypeDef GPIO_InitStructure;
 	USART_InitTypeDef USART_InitStructure;
 	NVIC_InitTypeDef NVIC_InitStructure;
 	 
 	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART2,ENABLE);	//ʹ��USART2ʱ��
 	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOA,ENABLE);	//ʹ��GPIOAʱ��
  	USART_DeInit(USART2);  //��λ����2
 	 //USART2_TX   PA.2
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_2; //PA.2
     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
     GPIO_Init(GPIOA, &GPIO_InitStructure); //��ʼ��PA2
    
     //USART2_RX	  PA.3
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_3;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
     GPIO_Init(GPIOA, &GPIO_InitStructure);  //��ʼ��PA3


    //USART ��ʼ������

 	USART_InitStructure.USART_BaudRate = bound;//һ������Ϊ9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//�ֳ�Ϊ8λ���ݸ�ʽ
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//һ��ֹͣλ
 	USART_InitStructure.USART_Parity = USART_Parity_No;//����żУ��λ
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//��Ӳ������������
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//�շ�ģʽ

     USART_Init(USART2, &USART_InitStructure); //��ʼ������
 #if EN_USART2_RX		  //���ʹ���˽���  
    //Usart2 NVIC ����
     NVIC_InitStructure.NVIC_IRQChannel = USART2_IRQn;
 	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//��ռ���ȼ�3
 	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//�����ȼ�3
 	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQͨ��ʹ��
 	NVIC_Init(&NVIC_InitStructure);	//����ָ���Ĳ�����ʼ��VIC�Ĵ���
    
     USART_ITConfig(USART2, USART_IT_RXNE, ENABLE);//�����ж�
 #endif
     USART_Cmd(USART2, ENABLE);                    //ʹ�ܴ��� 

 }

 void uart3_init(u32 bound){
	//GPIO�˿�����
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3, ENABLE);	//ʹ��USART3ʱ��

//	//����ӳ��
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB, ENABLE);   
//	
//	//������ӳ��
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC, ENABLE);    
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//ʹ��USART3ʱ��
//	GPIO_PinRemapConfig(GPIO_PartialRemap_USART3,ENABLE);
	
	//��ȫ��ӳ��
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOD, ENABLE);    
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//ʹ��USART3ʱ��
	GPIO_PinRemapConfig(GPIO_FullRemap_USART3,ENABLE);
	
	USART_DeInit(USART3);  //��λ����3
 	
 	/*USART3û����ӳ��  */
 	 //USART3_TX   PB.10
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
     GPIO_Init(GPIOB, &GPIO_InitStructure); //��ʼ��PB10
    
     //USART3_RX	  PB.11
     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
     GPIO_Init(GPIOB, &GPIO_InitStructure);  //��ʼ��PB11

 	
 	/*������ӳ��*/
 // 	 //USART3_TX   PC.10
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
 //     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
 //     GPIO_Init(GPIOC, &GPIO_InitStructure); //��ʼ��PC10
 //    
 //     //USART3_RX	  PC.11
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
 //     GPIO_Init(GPIOC, &GPIO_InitStructure);  //��ʼ��PC11
 	
 	/*��ȫ��ӳ��*/
	//USART3_TX   PD.8
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8; //PD.8
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
	GPIO_Init(GPIOD, &GPIO_InitStructure); //��ʼ��PD8
	
	//USART3_RX	  PD.9
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
	GPIO_Init(GPIOD, &GPIO_InitStructure);  //��ʼ��PD9

    //USART ��ʼ������

 	USART_InitStructure.USART_BaudRate = bound;//һ������Ϊ9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//�ֳ�Ϊ8λ���ݸ�ʽ
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//һ��ֹͣλ
 	USART_InitStructure.USART_Parity = USART_Parity_No;//����żУ��λ
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//��Ӳ������������
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//�շ�ģʽ

     USART_Init(USART3, &USART_InitStructure); //��ʼ������
 #if EN_USART3_RX		  //���ʹ���˽���  
    //Usart3 NVIC ����
	NVIC_InitStructure.NVIC_IRQChannel = USART3_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//��ռ���ȼ�3
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//�����ȼ�3
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQͨ��ʹ��
	NVIC_Init(&NVIC_InitStructure);	//����ָ���Ĳ�����ʼ��VIC�Ĵ���
	
	USART_ITConfig(USART3, USART_IT_RXNE, ENABLE);//�����ж�
 #endif
     USART_Cmd(USART3, ENABLE);                    //ʹ�ܴ��� 

 }
 //û����ӳ��
void uart3_init2(u32 bound){
	//GPIO�˿�����
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3, ENABLE);	//ʹ��USART3ʱ��
	
	  
	//����ӳ��  һ��Ҫ����ӳ�䰡��������
	GPIO_PinRemapConfig(GPIO_FullRemap_USART3,DISABLE);
	GPIO_PinRemapConfig(GPIO_PartialRemap_USART3,DISABLE); 
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB, ENABLE);


	
//	//������ӳ��
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC, ENABLE);    
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//ʹ��USART3ʱ��
//	GPIO_PinRemapConfig(GPIO_PartialRemap_USART3,ENABLE);
//	
//	//��ȫ��ӳ��
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOD, ENABLE);    
//	RCC_APB2PeriphClockCmd(RCC_APB2Periph_AFIO, ENABLE);	//ʹ��USART3ʱ��
//	GPIO_PinRemapConfig(GPIO_FullRemap_USART3,ENABLE);
	
	USART_DeInit(USART3);  //��λ����3
	
	/*USART3û����ӳ��  */
	//USART3_TX   PB.10
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
	GPIO_Init(GPIOB, &GPIO_InitStructure); //��ʼ��PB10
	
	//USART3_RX	  PB.11
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
	GPIO_Init(GPIOB, &GPIO_InitStructure);  //��ʼ��PB11

 	
 	/*������ӳ��*/
 // 	 //USART3_TX   PC.10
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PB.10
 //     GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
 //     GPIO_Init(GPIOC, &GPIO_InitStructure); //��ʼ��PC10
 //    
 //     //USART3_RX	  PC.11
 //     GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
 //     GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
 //     GPIO_Init(GPIOC, &GPIO_InitStructure);  //��ʼ��PC11
 	
 	/*��ȫ��ӳ��*/
//	//USART3_TX   PD.8
//	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8; //PD.8
//	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
//	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
//	GPIO_Init(GPIOD, &GPIO_InitStructure); //��ʼ��PD8
//	
//	//USART3_RX	  PD.9
//	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;
//	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
//	GPIO_Init(GPIOD, &GPIO_InitStructure);  //��ʼ��PD9

    //USART ��ʼ������

 	USART_InitStructure.USART_BaudRate = bound;//һ������Ϊ9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//�ֳ�Ϊ8λ���ݸ�ʽ
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//һ��ֹͣλ
 	USART_InitStructure.USART_Parity = USART_Parity_No;//����żУ��λ
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//��Ӳ������������
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//�շ�ģʽ

     USART_Init(USART3, &USART_InitStructure); //��ʼ������
 #if EN_USART3_RX		  //���ʹ���˽���  
    //Usart3 NVIC ����
	NVIC_InitStructure.NVIC_IRQChannel = USART3_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//��ռ���ȼ�3
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//�����ȼ�3
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQͨ��ʹ��
	NVIC_Init(&NVIC_InitStructure);	//����ָ���Ĳ�����ʼ��VIC�Ĵ���
	
	USART_ITConfig(USART3, USART_IT_RXNE, ENABLE);//�����ж�
 #endif
     USART_Cmd(USART3, ENABLE);                    //ʹ�ܴ��� 

 }

 void uart4_init(u32 bound){
     //GPIO�˿�����
	GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOC, ENABLE);	//ʹ��GPIOCʱ��
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_UART4,ENABLE);
	USART_DeInit(UART4);  //��λ����4
	//USART4_TX   PC.10
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10; //PC.10
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
	GPIO_Init(GPIOC, &GPIO_InitStructure); //��ʼ��PC10
	
	//USART4_RX	  PC.11
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_11;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
	GPIO_Init(GPIOC, &GPIO_InitStructure);  //��ʼ��PC11


    //USART ��ʼ������

 	USART_InitStructure.USART_BaudRate = bound;//һ������Ϊ9600;
 	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//�ֳ�Ϊ8λ���ݸ�ʽ
 	USART_InitStructure.USART_StopBits = USART_StopBits_1;//һ��ֹͣλ
 	USART_InitStructure.USART_Parity = USART_Parity_No;//����żУ��λ
 	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//��Ӳ������������
 	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//�շ�ģʽ

     USART_Init(UART4, &USART_InitStructure); //��ʼ������
 #if EN_USART4_RX		  //���ʹ���˽���  
    //Usart4 NVIC ����
     NVIC_InitStructure.NVIC_IRQChannel = UART4_IRQn;
 	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//��ռ���ȼ�3
 	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//�����ȼ�3
 	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQͨ��ʹ��
 	NVIC_Init(&NVIC_InitStructure);	//����ָ���Ĳ�����ʼ��VIC�Ĵ���
    
     USART_ITConfig(UART4, USART_IT_RXNE, ENABLE);//�����ж�
 #endif
     USART_Cmd(UART4, ENABLE);                    //ʹ�ܴ��� 

 }

void uart5_init(u32 bound){
    //GPIO�˿�����
    GPIO_InitTypeDef GPIO_InitStructure;
	USART_InitTypeDef USART_InitStructure;
	NVIC_InitTypeDef NVIC_InitStructure;
	 
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_UART5, ENABLE);	//ʹ��GPIOAʱ��
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOD|RCC_APB2Periph_GPIOC, ENABLE);
 	USART_DeInit(UART5);  //��λ����1
	 //USART5_TX   PC12
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_12; //PC.12
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF_PP;	//�����������
    GPIO_Init(GPIOC, &GPIO_InitStructure); //��ʼ��PC.12
   
    //USART5_RX	  PD.2
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_2;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;//��������
    GPIO_Init(GPIOD, &GPIO_InitStructure);  //��ʼ��PD2


   //USART ��ʼ������

	USART_InitStructure.USART_BaudRate = bound;//һ������Ϊ9600;
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;//�ֳ�Ϊ8λ���ݸ�ʽ
	USART_InitStructure.USART_StopBits = USART_StopBits_1;//һ��ֹͣλ
	USART_InitStructure.USART_Parity = USART_Parity_No;//����żУ��λ
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;//��Ӳ������������
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;	//�շ�ģʽ

    USART_Init(UART5, &USART_InitStructure); //��ʼ������
#if EN_USART5_RX		  //���ʹ���˽���  
   //Usart5 NVIC ����
    NVIC_InitStructure.NVIC_IRQChannel = UART5_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority=3 ;//��ռ���ȼ�3
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 3;		//�����ȼ�3
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;			//IRQͨ��ʹ��
	NVIC_Init(&NVIC_InitStructure);	//����ָ���Ĳ�����ʼ��VIC�Ĵ���
   
    USART_ITConfig(UART5, USART_IT_RXNE, ENABLE);//�����ж�
#endif
    USART_Cmd(UART5, ENABLE);                    //ʹ�ܴ��� 

}

int send_byte_to_usart1(char data)
{
	USART_SendData(USART1, data);//�򴮿�5��������
	while(USART_GetFlagStatus(USART1,USART_FLAG_TC)!=SET);//�ȴ����ͽ���
	return 0;
}
					
int send_byte_to_usart2(char data)
{
	USART_SendData(USART2, data);//�򴮿�5��������
	while(USART_GetFlagStatus(USART2,USART_FLAG_TC)!=SET);//�ȴ����ͽ���
	return 0;
}

int send_byte_to_usart3(char data)
{
	USART_SendData(USART3, data);//�򴮿�5��������
	while(USART_GetFlagStatus(USART3,USART_FLAG_TC)!=SET);//�ȴ����ͽ���
	return 0;
}

int send_byte_to_usart4(char data)
{
	USART_SendData(UART4, data);//�򴮿�5��������
	while(USART_GetFlagStatus(UART4,USART_FLAG_TC)!=SET);//�ȴ����ͽ���
	return 0;
}	

int send_byte_to_usart5(char data)
{
	USART_SendData(UART5, data);//�򴮿�5��������
	while(USART_GetFlagStatus(UART5,USART_FLAG_TC)!=SET);//�ȴ����ͽ���
	return 0;
}

/**********************************************************************/
//   ����Ϊ�����ַ���
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
//   ����Ϊ��������
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
char PC_cmd;//01 Ϊ��ѯ״̬  10-14 ��ȡ�궨����	20-24д��궨����
char PC_Wcmd;
u16 load;

/*******************************************************************************/	  
//
//								����λ��ͨѶ
/*******************************************************************************/

void USART1_IRQHandler(void)                	//����1�жϷ������
{
	u8 Res;
	char* token;
	
	if(USART_GetITStatus(USART1, USART_IT_RXNE) != RESET)  //�����ж�
	{
		Res =USART_ReceiveData(USART1);//(USART1->DR);	//��ȡ���յ�������

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
			//�궨ѹ��0λ
			if(USART_RX_BUF[0][0]=='3'                            && USART_RX_BUF[0][2]==',' )
			{

				PC_Wcmd = 0x30; 	
			}
			//�궨ѹ������
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
//								��ѯ����
/*******************************************************************************/
void USART2_IRQHandler(void)                	//����2�жϷ������  SD�� ����
{
	//���շ���
	int i;
	if(USART_GetITStatus(USART2, USART_IT_RXNE) != RESET)  //�����ж�
	{
		USART_RX_BUF[1][12] =USART_ReceiveData(USART2);	//��ȡ���յ�������
		
		//˳����ǰ��
		for(i=0;i<12;i++)
		{
			USART_RX_BUF[1][i] = USART_RX_BUF[1][i+1];	
		}

		//��ַ1 �Ƿ���
		if(USART_RX_BUF[1][0] == 0x01 && USART_RX_BUF[1][1] == 0x03 && USART_RX_BUF[1][2] == 0x02)
		//if(USART_RX_BUF[1][0] == 0x00 && USART_RX_BUF[1][1] == 0x10 && USART_RX_BUF[1][2] == 0x01)
		{
			int crc16 = 0;
			char crcFirst=0,crcAfter = 0;
			crc16 = getCRC16_LowFirst(USART_RX_BUF[1],5);
			crcFirst = (crc16 & (0xFF<<8))>>8;
			crcAfter = 	crc16;
			//printf("U2-2\r\n");
			if(USART_RX_BUF[1][5] == crcFirst && USART_RX_BUF[1][6] == crcAfter)//CRCͨ��
			{		
				g_WindSpeed = USART_RX_BUF[1][3];
				g_WindSpeed = (g_WindSpeed<<8) + USART_RX_BUF[1][4];
				g_Temper = DS18B20_Get_Temp(); //תһ�°�
				//printf("U2-3\r\n");
			}
		}
	}
 }
 
/*******************************************************************************/	  
//
//								��дSD�� ��ѯ����ȡ��
/*******************************************************************************/ 
extern char g_uart3_used_for_SD;
void USART3_IRQHandler(void)                	//����4�жϷ������
{
	char temp;
	int i;
	if(USART_GetITStatus(USART3, USART_IT_RXNE) != RESET)  //�����ж�
	{

		temp =USART_ReceiveData(USART3);
		if(F_wait_response == 0x01)
		{
			SD_cmd_return_byte = temp;
			F_wait_response = 0;
		}
	}	
} 
//����
void UART4_IRQHandler(void)                	//����4�жϷ������
{
	int i;
	int Pa;
	char weight[4];
	if(USART_GetITStatus(UART4, USART_IT_RXNE) != RESET)  //�����ж�
	{
		USART_RX_BUF[3][USART_REC_LEN-1] =USART_ReceiveData(UART4);	//��ȡ���յ�������
		
		//˳����ǰ��
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
			if(USART_RX_BUF[3][7] == crcFirst && USART_RX_BUF[3][8] == crcAfter)//CRCͨ��
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
		//���궨
		if(USART_RX_BUF[3][0] == 0x01 && USART_RX_BUF[3][1] == 0x06 
			&& USART_RX_BUF[3][2] == 0x00 && USART_RX_BUF[3][3] == 0x12
			&& USART_RX_BUF[3][4] == 0x00 && USART_RX_BUF[3][5] == 0x01
			&& USART_RX_BUF[3][6] == 0xE8 && USART_RX_BUF[3][7] == 0x0F)
		{
		}
		//���ر궨1
		if(USART_RX_BUF[3][0] == 0x01 && USART_RX_BUF[3][1] == 0x06 
			&& USART_RX_BUF[3][2] == 0x00 && USART_RX_BUF[3][3] == 0x12
			&& USART_RX_BUF[3][4] == 0x00 && USART_RX_BUF[3][5] == 0x02
			&& USART_RX_BUF[3][6] == 0xA8 && USART_RX_BUF[3][7] == 0x0E)
		{
			PC_Wcmd = 0x41;	
		}
		//���ر궨2
		if(USART_RX_BUF[3][0] == 0x01 && USART_RX_BUF[3][1] == 0x10 
			&& USART_RX_BUF[3][2] == 0x00 && USART_RX_BUF[3][3] == 0x08
			&& USART_RX_BUF[3][4] == 0x00 && USART_RX_BUF[3][5] == 0x02)
		{
		}
	}
}  	

/*******************************************************************************/	  
//                           ��ȡ���
//
//                 |��ʼ�Ĵ���|	4���Ĵ��� 4·AD| crcУ�� ��λ��ǰ|
// ��AD ���� 01 03   00 68       00 04			  44 03
//              |0x30 Ϊ��9·AD| 

//                | �յ�8�ֽ� |	��9· |��10·|��11·|��12·| У��
// �յ�      01 03      08 	    XX XX  XX XX  XX XX  XX XX

/*******************************************************************************/
void UART5_IRQHandler(void)                	//����5�жϷ������
{
	int i;
	char ind[8];
	if(USART_GetITStatus(UART5, USART_IT_RXNE) != RESET)  //�����ж�
	{
		USART_RX_BUF[4][USART_REC_LEN-1] =USART_ReceiveData(UART5);//(USART1->DR);	//��ȡ���յ�������
		
		//˳����ǰ��
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
			if(USART_RX_BUF[4][11] == crcFirst && USART_RX_BUF[4][12] == crcAfter)//CRCͨ��
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

