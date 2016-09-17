#ifndef __SD_CARD_H
#define __SD_CARD_H	
#include "stm32f10x.h"

#define uchar unsigned char
#define uint unsigned int

#define SYS 0x01
#define SD_CREATE 0x02
#define SD_OPEN 0x06
#define SD_WRITE 0x05
#define SD_SAVE 0x04

//#define IS_SD_BUSY GPIO_ReadInputDataBit(GPIOE,GPIO_Pin_4)//SD����busy����
#define IS_SD_BUSY GPIO_ReadInputDataBit(GPIOB,GPIO_Pin_14)//SD����busy����
#define OPEN_SD_POWER GPIO_SetBits(GPIOB,GPIO_Pin_15)		//SD���ĵ�Դ
#define CLOSE_SD_POWER GPIO_ResetBits(GPIOB,GPIO_Pin_15)	//SD���ĵ�Դ


void SD_init(void);
void SD_read_system(void);//��ȡϵͳ״̬
void SD_save_file(void);//�����ļ�
void SD_close_file(void);//�ر��ļ�


//��SD����������
void SD_send_cmd(uchar command/*������*/,uint number/*���ݳ���*/,uchar *date/*����������*/);
//��SD����������
void SD_send_data(uchar command/*������*/,uchar number/*���ݳ���*/,uint addr/*4���ֽڵĵ�ַ*/,uchar *date/*����������*/);

void Send_UART(unsigned int num,unsigned char *Date_OUT);
void Send_UART_byte(unsigned char Date);
#endif
