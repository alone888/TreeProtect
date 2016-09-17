/*************************************************************************************/
/*     程序名：51单片机的串口---SDV600模块测试程序                                   */
/*                                                                                   */
/*     晶振：22.1184 M           波特率115200              CPU型号：89C52            */
/*                                                                                   */
/*                                                                                   */
/*     功能描述：单片机上电后，在SD卡中建立文件mydata01.txt和mydata02.txt，			 */
/*               并写入十组数据DATE_SEND 											 */
/*                                                                                   */
/*     http://www.botom.cn 															 */
/*     http://www.prog430.com                                                        */
/*     http://www.prog430.com/bbs                                                    */
/*   													 					    	 */
/*                                                                                   */
/*                                                   北京博通电子   版权所有         */ 
/*                                                                                   */    
/*                                                         2009-08-20                */
/*************************************************************************************/ 

#include "usart.h"
#include "led.h"
#include "sd_card.h"
#include <string.h>



//sbit P1_0 = P1 ^ 0;//busy忙标志管脚
//sbit P1_1 = P1 ^ 1;//SD模块电源
//sbit P1_5 = P1 ^ 5;//测试板信号指示（指示发送数据）



uchar FileName[]={"mydata01.txt"};  //要建立的文件名
static uchar  Cmd_header[2]={0x55,0xAA}; //命令头
static uchar  SYS_state[6]={0x55,0xAA,0x01,0x00,0x00,0x01};//获取系统状态

//static uchar  Cre_files[19]={0x55,0xAA,0x02,0x0D,0x00,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x2E,0x74,0x78,0x74,0x00,0x41};
//static uchar  Open_files[19]={0x55,0xAA,0x06,0x0D,0x00,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x2E,0x74,0x78,0x74,0x00,0x45};
//static uchar  Wri_file[19]={0x55,0xAA,0x05,0x0D,0x00,0x00,0x00,0x00,0x00,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0xef,};
static uchar  Sav_files[6]={0x55,0xAA,0x04,0x00,0x00,0x04};
static uchar  Close_files[6]={0x55,0xAA,0x08,0x00,0x00,0x08};
uchar  Temp_data[30]={0};
uchar DATE_SEND[] = {"09-08-07 25 55 878 "};


void Send_UART(unsigned int num,unsigned char *Date_OUT)
{
  	send_buf_to_usart3(Date_OUT,num);
}
void Send_UART_byte(unsigned char Date)
{
	send_byte_to_usart3(Date);
}

/***********************************************************************************************/
void Delay(unsigned int a)
{
	unsigned char i;
	while( --a != 0)
	{
		for(i = 0; i < 3; i++);
	}
}

void SD_init()//主要是busy线的判断
{
	GPIO_InitTypeDef GPIO_InitStructure;
	/*********************输入口设置****************************************************************/
   	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_4;				 //LED0-->PE.4  端口配置
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING; 	 //输入
	GPIO_Init(GPIOE, &GPIO_InitStructure);					 //根据设定参数初始化GPIOE.4
	GPIO_ReadInputDataBit(GPIOE,GPIO_Pin_4);				 //读取引脚状态
	/***********************************************************************************************/
	/*********************输入口设置****************************************************************/
   	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_14;				 //LED0-->PE.4  端口配置
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING; 	 //输入
	GPIO_Init(GPIOB, &GPIO_InitStructure);					 //根据设定参数初始化GPIOE.4
	GPIO_ReadInputDataBit(GPIOB,GPIO_Pin_14);				 //读取引脚状态
	/***********************************************************************************************/

	/*********************输出口设置****************************************************************/	
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_15;				 //LED0-->PE.6  端口配置
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 		 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOB, &GPIO_InitStructure);					 //根据设定参数初始化GPIOB.5
	GPIO_SetBits(GPIOB,GPIO_Pin_15);							 //PE.6 输出高
	
	GPIO_ResetBits(GPIOB,GPIO_Pin_15);

}

//void main (void)
//{
//    uint i;
//
//	DATE_SEND[18] = 0x0a;
// 	P1_1 = 1;//给SD卡模块上电
//
//	SCON    =   0x50;   //0101,0000 串口方式1，允许接收，无奇偶校验位
//    TMOD    =   0x20;   //0010,0000 时器1，定时方式2 自动重装装计数器
//    PCON = 0x80;  //SMOD=1
//	TL1 = 0xff;
//	TH1 = 0xff;
//	TR1 = 1;
//    Delay(5000);//上电延时
//
//  
//  //①获取系统状态       ⑥⑦⑧⑨
//  //----------------------------------------------------------------------------
//  SYS_R();//读取系统状态
//  Delay(1);
//  while(P1_0);
//  
//  //②创建文件
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Cre,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //③打开文件
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Open,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //④写入文件
//  //----------------------------------------------------------------------------
//   
//  for(i=0;i<10;i++)
//  {
//    Sto_SD_data(Wri,23,0xffffffff,DATE_SEND);
//	Delay(1);
//    while(P1_0);
//  }
//
//  //⑤保存文件
//  //---------------------------------------------------------------------------- 
//  save_file();//保存文件
//  Delay(1);
//  while(P1_0);
//
//  //⑥关闭文件
//  //---------------------------------------------------------------------------- 
//  Close_file();//关闭文件
//  Delay(1);
//  while(P1_0);
//  
//
//
//
//
//
////222222222222222222222222222222222222222222222222222222222222222222222222222222
////创建第二个文件
//  FileName[7]=0x32;//"2"
//  //①获取系统状态       ⑥⑦⑧⑨
//  //----------------------------------------------------------------------------
//  SYS_R();//读取系统状态
//  Delay(1);
//  while(P1_0);
//  
//  //②创建文件
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Cre,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //③打开文件
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Open,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //④写入文件
//  //----------------------------------------------------------------------------
//   
//  for(i=0;i<10;i++)
//  {
//    Sto_SD_data(Wri,23,0xffffffff,DATE_SEND);
//	Delay(1);
//    while(P1_0);
//  }
//
//  //⑤保存文件
//  //---------------------------------------------------------------------------- 
//  save_file();//保存文件
//  Delay(1);
//  while(P1_0);
//
//  //⑥关闭文件
//  //---------------------------------------------------------------------------- 
//  Close_file();//关闭文件
//  Delay(1);
//  while(P1_0);
//
//  while(1);
//}


void SD_send_cmd_header()
{
	Send_UART(2,Cmd_header);
}

			 
//向SD卡发送命令
void SD_send_cmd(uchar command/*命令字*/,uint number/*数据长度*/,uchar *date/*待发送数据*/)
{ 
	uint i=0;
	uchar crc=0;
	uchar dataLenH,dataLenL;

	dataLenL = number%256;
	dataLenH = number/256;

	LED0 = 0;//点亮测试板指示灯
	BLED1 = 1;
	
	Send_UART(2,Cmd_header);

	
	//发送命令字
	Send_UART(1,&command);
	crc += command;
	
	
	//发送数据长度
	Send_UART(1,&dataLenL);
	crc += dataLenL;
	
	Send_UART(1,&dataLenH);
	crc += dataLenH;
	
	//发送数据--串口0
	Send_UART(number,date);
	i=0;
	while(i<number)
	{
		crc += date[i];
		i++;
	}
	
	//发送校验和
	Send_UART_byte(crc);
	
	LED0 = 1;//熄灭测试板指示灯
	BLED1 = 0;
}


//向SD卡发送数据 /*命令字*//*数据长度*//*4个字节的地址*//*待发送数据*/
void SD_send_data(uchar command,uchar number,uint addr,uchar *date)
{ 
	unsigned int i=0;
	unsigned char crc=0;
	
	LED0 = 0;//点亮测试板指示灯
	BLED1 = 1;
	
	Send_UART(2,Cmd_header);
	
	
	//发送命令字
	Send_UART_byte(command);
	crc += command;
	
	
	//发送数据长度
	Send_UART_byte(number%256);
	crc += (number%256);

	Send_UART_byte(number/256);
	crc += (number/256);
	
	//发送地址
	Send_UART_byte(addr%256);
	Send_UART_byte((addr/256)%256);
	Send_UART_byte(((addr/256)/256)%256);
	Send_UART_byte(((addr/256)/256)/256);

	crc += (addr%256);
	crc += (addr/256)%256;
	crc += ((addr/256)/256)%256;
	crc += ((addr/256)/256)/256;
	
	
	//发送数据--串口0
	
	Send_UART(number-4,date);
	i=0;
	while(i<number-4)
	{
		crc += date[i];
		i++;
	}
	
	//发送校验和
	Send_UART_byte(crc);
		
	LED0 = 1;//熄灭测试板指示灯
	BLED1 = 0;
}


void SD_read_system()//读取系统状态
{
  memcpy(Temp_data,SYS_state,6);
  Send_UART(6,Temp_data); 
}


void SD_save_file()//保存文件
{
  memcpy(Temp_data,Sav_files,6);
  Send_UART(6,Temp_data);
}


void SD_close_file()//关闭文件
{
  memcpy(Temp_data,Close_files,6);
  Send_UART(6,Temp_data);
}
