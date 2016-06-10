#ifndef __DS18B20_H
#define __DS18B20_H 
  
#include "stm32f10x.h"

#define DS18B20_DQ_OUT_Low GPIO_ResetBits(GPIOG,GPIO_Pin_9)  //���ݶ˿�	PA0 
#define DS18B20_DQ_OUT_High GPIO_SetBits(GPIOG,GPIO_Pin_9)  //���ݶ˿�	PA0 
#define DS18B20_DQ_IN GPIO_ReadInputDataBit(GPIOG,GPIO_Pin_9)   //���ݶ˿�	PA0

void DS18B20_GPIO_Config(void);
void DS18B20_Mode_IPU(void);
void DS18B20_Mode_Out(void);
u8 DS18B20_Init(void);//��ʼ��DS18B20
short DS18B20_Get_Temp(void);//��ȡ�¶�

void DS18B20_Write_Byte(u8 dat);//д��һ���ֽ�
u8 DS18B20_Read_Byte(void);//����һ���ֽ�
u8 DS18B20_Read_Bit(void);//����һ��λ
u8 DS18B20_Answer_Check(void);//����Ƿ����DS18B20
void DS18B20_Rst(void);//��λDS18B20    
#endif















