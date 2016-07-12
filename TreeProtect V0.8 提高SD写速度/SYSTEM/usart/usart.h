#ifndef __USART_H
#define __USART_H
#include "stdio.h"	
#include "sys.h" 
//////////////////////////////////////////////////////////////////////////////////	 

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
////////////////////////////////////////////////////////////////////////////////// 	
#define USART_REC_LEN  			30  	//定义最大接收字节数 200
#define EN_USART1_RX 			1		//使能（1）/禁止（0）串口1接收
#define EN_USART2_RX 			1		//使能（1）/禁止（0）串口1接收
#define EN_USART3_RX 			1		//使能（1）/禁止（0）串口1接收
#define EN_USART4_RX 			1		//使能（1）/禁止（0）串口1接收
#define EN_USART5_RX 			1		//使能（1）/禁止（0）串口1接收
	  	
extern char  USART_RX_BUF[5][USART_REC_LEN]; //接收缓冲,最大USART_REC_LEN个字节.末字节为换行符 
extern u16 USART_RX_STA;         		//接收状态标记
extern char PC_cmd,PC_Wcmd;//上位机发送的普通命令 和重量标定命令



	
//如果想串口中断接收，请不要注释以下宏定义
void uart1_init(u32 bound);
void uart2_init(u32 bound);
void uart3_init(u32 bound);
void uart3_init2(u32 bound);//重映射的初始化
void uart4_init(u32 bound);
void uart5_init(u32 bound);

int send_byte_to_usart1(char data);
int send_byte_to_usart2(char data);
int send_byte_to_usart3(char data);
int send_byte_to_usart4(char data);
int send_byte_to_usart5(char data);

int send_buf_to_usart1(unsigned char * str, unsigned int buflen);
int send_buf_to_usart2(unsigned char * str, unsigned int buflen);
int send_buf_to_usart3(unsigned char * str, unsigned int buflen);
int send_buf_to_usart4(unsigned char * str, unsigned int buflen);
int send_buf_to_usart4(unsigned char * str, unsigned int buflen);

int send_string_to_usart1(char * str);
int send_string_to_usart2(char * str);
int send_string_to_usart3(char * str);
int send_string_to_usart4(char * str);
int send_string_to_usart5(char * str);

#endif


