#ifndef __USART_H
#define __USART_H
#include "stdio.h"	
#include "sys.h" 
//////////////////////////////////////////////////////////////////////////////////	 

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
////////////////////////////////////////////////////////////////////////////////// 	
#define USART_REC_LEN  			30  	//�����������ֽ��� 200
#define EN_USART1_RX 			1		//ʹ�ܣ�1��/��ֹ��0������1����
#define EN_USART2_RX 			1		//ʹ�ܣ�1��/��ֹ��0������1����
#define EN_USART3_RX 			1		//ʹ�ܣ�1��/��ֹ��0������1����
#define EN_USART4_RX 			1		//ʹ�ܣ�1��/��ֹ��0������1����
#define EN_USART5_RX 			1		//ʹ�ܣ�1��/��ֹ��0������1����
	  	
extern char  USART_RX_BUF[5][USART_REC_LEN]; //���ջ���,���USART_REC_LEN���ֽ�.ĩ�ֽ�Ϊ���з� 
extern u16 USART_RX_STA;         		//����״̬���
extern char PC_cmd,PC_Wcmd;//��λ�����͵���ͨ���� �������궨����



	
//����봮���жϽ��գ��벻Ҫע�����º궨��
void uart1_init(u32 bound);
void uart2_init(u32 bound);
void uart3_init(u32 bound);
void uart3_init2(u32 bound);//��ӳ��ĳ�ʼ��
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


