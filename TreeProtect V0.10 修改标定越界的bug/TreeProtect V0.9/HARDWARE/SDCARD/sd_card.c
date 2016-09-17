/*************************************************************************************/
/*     ��������51��Ƭ���Ĵ���---SDV600ģ����Գ���                                   */
/*                                                                                   */
/*     ����22.1184 M           ������115200              CPU�ͺţ�89C52            */
/*                                                                                   */
/*                                                                                   */
/*     ������������Ƭ���ϵ����SD���н����ļ�mydata01.txt��mydata02.txt��			 */
/*               ��д��ʮ������DATE_SEND 											 */
/*                                                                                   */
/*     http://www.botom.cn 															 */
/*     http://www.prog430.com                                                        */
/*     http://www.prog430.com/bbs                                                    */
/*   													 					    	 */
/*                                                                                   */
/*                                                   ������ͨ����   ��Ȩ����         */ 
/*                                                                                   */    
/*                                                         2009-08-20                */
/*************************************************************************************/ 

#include "usart.h"
#include "led.h"
#include "sd_card.h"
#include <string.h>



//sbit P1_0 = P1 ^ 0;//busyæ��־�ܽ�
//sbit P1_1 = P1 ^ 1;//SDģ���Դ
//sbit P1_5 = P1 ^ 5;//���԰��ź�ָʾ��ָʾ�������ݣ�



uchar FileName[]={"mydata01.txt"};  //Ҫ�������ļ���
static uchar  Cmd_header[2]={0x55,0xAA}; //����ͷ
static uchar  SYS_state[6]={0x55,0xAA,0x01,0x00,0x00,0x01};//��ȡϵͳ״̬

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

void SD_init()//��Ҫ��busy�ߵ��ж�
{
	GPIO_InitTypeDef GPIO_InitStructure;
	/*********************���������****************************************************************/
   	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_4;				 //LED0-->PE.4  �˿�����
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING; 	 //����
	GPIO_Init(GPIOE, &GPIO_InitStructure);					 //�����趨������ʼ��GPIOE.4
	GPIO_ReadInputDataBit(GPIOE,GPIO_Pin_4);				 //��ȡ����״̬
	/***********************************************************************************************/
	/*********************���������****************************************************************/
   	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_14;				 //LED0-->PE.4  �˿�����
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING; 	 //����
	GPIO_Init(GPIOB, &GPIO_InitStructure);					 //�����趨������ʼ��GPIOE.4
	GPIO_ReadInputDataBit(GPIOB,GPIO_Pin_14);				 //��ȡ����״̬
	/***********************************************************************************************/

	/*********************���������****************************************************************/	
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_15;				 //LED0-->PE.6  �˿�����
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 		 //�������
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO���ٶ�Ϊ50MHz
	GPIO_Init(GPIOB, &GPIO_InitStructure);					 //�����趨������ʼ��GPIOB.5
	GPIO_SetBits(GPIOB,GPIO_Pin_15);							 //PE.6 �����
	
	GPIO_ResetBits(GPIOB,GPIO_Pin_15);

}

//void main (void)
//{
//    uint i;
//
//	DATE_SEND[18] = 0x0a;
// 	P1_1 = 1;//��SD��ģ���ϵ�
//
//	SCON    =   0x50;   //0101,0000 ���ڷ�ʽ1��������գ�����żУ��λ
//    TMOD    =   0x20;   //0010,0000 ʱ��1����ʱ��ʽ2 �Զ���װװ������
//    PCON = 0x80;  //SMOD=1
//	TL1 = 0xff;
//	TH1 = 0xff;
//	TR1 = 1;
//    Delay(5000);//�ϵ���ʱ
//
//  
//  //�ٻ�ȡϵͳ״̬       �ޢߢ��
//  //----------------------------------------------------------------------------
//  SYS_R();//��ȡϵͳ״̬
//  Delay(1);
//  while(P1_0);
//  
//  //�ڴ����ļ�
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Cre,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //�۴��ļ�
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Open,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //��д���ļ�
//  //----------------------------------------------------------------------------
//   
//  for(i=0;i<10;i++)
//  {
//    Sto_SD_data(Wri,23,0xffffffff,DATE_SEND);
//	Delay(1);
//    while(P1_0);
//  }
//
//  //�ݱ����ļ�
//  //---------------------------------------------------------------------------- 
//  save_file();//�����ļ�
//  Delay(1);
//  while(P1_0);
//
//  //�޹ر��ļ�
//  //---------------------------------------------------------------------------- 
//  Close_file();//�ر��ļ�
//  Delay(1);
//  while(P1_0);
//  
//
//
//
//
//
////222222222222222222222222222222222222222222222222222222222222222222222222222222
////�����ڶ����ļ�
//  FileName[7]=0x32;//"2"
//  //�ٻ�ȡϵͳ״̬       �ޢߢ��
//  //----------------------------------------------------------------------------
//  SYS_R();//��ȡϵͳ״̬
//  Delay(1);
//  while(P1_0);
//  
//  //�ڴ����ļ�
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Cre,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //�۴��ļ�
//  //----------------------------------------------------------------------------
//  Sto_SD_cmd(Open,13,FileName);
//  Delay(1);
//  while(P1_0);
//  
//  //��д���ļ�
//  //----------------------------------------------------------------------------
//   
//  for(i=0;i<10;i++)
//  {
//    Sto_SD_data(Wri,23,0xffffffff,DATE_SEND);
//	Delay(1);
//    while(P1_0);
//  }
//
//  //�ݱ����ļ�
//  //---------------------------------------------------------------------------- 
//  save_file();//�����ļ�
//  Delay(1);
//  while(P1_0);
//
//  //�޹ر��ļ�
//  //---------------------------------------------------------------------------- 
//  Close_file();//�ر��ļ�
//  Delay(1);
//  while(P1_0);
//
//  while(1);
//}


void SD_send_cmd_header()
{
	Send_UART(2,Cmd_header);
}

			 
//��SD����������
void SD_send_cmd(uchar command/*������*/,uint number/*���ݳ���*/,uchar *date/*����������*/)
{ 
	uint i=0;
	uchar crc=0;
	uchar dataLenH,dataLenL;

	dataLenL = number%256;
	dataLenH = number/256;

	LED0 = 0;//�������԰�ָʾ��
	BLED1 = 1;
	
	Send_UART(2,Cmd_header);

	
	//����������
	Send_UART(1,&command);
	crc += command;
	
	
	//�������ݳ���
	Send_UART(1,&dataLenL);
	crc += dataLenL;
	
	Send_UART(1,&dataLenH);
	crc += dataLenH;
	
	//��������--����0
	Send_UART(number,date);
	i=0;
	while(i<number)
	{
		crc += date[i];
		i++;
	}
	
	//����У���
	Send_UART_byte(crc);
	
	LED0 = 1;//Ϩ����԰�ָʾ��
	BLED1 = 0;
}


//��SD���������� /*������*//*���ݳ���*//*4���ֽڵĵ�ַ*//*����������*/
void SD_send_data(uchar command,uchar number,uint addr,uchar *date)
{ 
	unsigned int i=0;
	unsigned char crc=0;
	
	LED0 = 0;//�������԰�ָʾ��
	BLED1 = 1;
	
	Send_UART(2,Cmd_header);
	
	
	//����������
	Send_UART_byte(command);
	crc += command;
	
	
	//�������ݳ���
	Send_UART_byte(number%256);
	crc += (number%256);

	Send_UART_byte(number/256);
	crc += (number/256);
	
	//���͵�ַ
	Send_UART_byte(addr%256);
	Send_UART_byte((addr/256)%256);
	Send_UART_byte(((addr/256)/256)%256);
	Send_UART_byte(((addr/256)/256)/256);

	crc += (addr%256);
	crc += (addr/256)%256;
	crc += ((addr/256)/256)%256;
	crc += ((addr/256)/256)/256;
	
	
	//��������--����0
	
	Send_UART(number-4,date);
	i=0;
	while(i<number-4)
	{
		crc += date[i];
		i++;
	}
	
	//����У���
	Send_UART_byte(crc);
		
	LED0 = 1;//Ϩ����԰�ָʾ��
	BLED1 = 0;
}


void SD_read_system()//��ȡϵͳ״̬
{
  memcpy(Temp_data,SYS_state,6);
  Send_UART(6,Temp_data); 
}


void SD_save_file()//�����ļ�
{
  memcpy(Temp_data,Sav_files,6);
  Send_UART(6,Temp_data);
}


void SD_close_file()//�ر��ļ�
{
  memcpy(Temp_data,Close_files,6);
  Send_UART(6,Temp_data);
}
