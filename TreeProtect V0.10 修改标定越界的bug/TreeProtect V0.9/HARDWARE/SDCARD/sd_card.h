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

//#define IS_SD_BUSY GPIO_ReadInputDataBit(GPIOE,GPIO_Pin_4)//SD卡的busy引脚
#define IS_SD_BUSY GPIO_ReadInputDataBit(GPIOB,GPIO_Pin_14)//SD卡的busy引脚
#define OPEN_SD_POWER GPIO_SetBits(GPIOB,GPIO_Pin_15)		//SD卡的电源
#define CLOSE_SD_POWER GPIO_ResetBits(GPIOB,GPIO_Pin_15)	//SD卡的电源


void SD_init(void);
void SD_read_system(void);//读取系统状态
void SD_save_file(void);//保存文件
void SD_close_file(void);//关闭文件


//向SD卡发送命令
void SD_send_cmd(uchar command/*命令字*/,uint number/*数据长度*/,uchar *date/*待发送数据*/);
//向SD卡发送数据
void SD_send_data(uchar command/*命令字*/,uchar number/*数据长度*/,uint addr/*4个字节的地址*/,uchar *date/*待发送数据*/);

void Send_UART(unsigned int num,unsigned char *Date_OUT);
void Send_UART_byte(unsigned char Date);
#endif
