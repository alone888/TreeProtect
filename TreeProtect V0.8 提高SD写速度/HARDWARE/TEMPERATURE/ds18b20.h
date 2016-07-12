#ifndef __DS18B20_H
#define __DS18B20_H 
  
#include "stm32f10x.h"

#define DS18B20_DQ_OUT_Low GPIO_ResetBits(GPIOG,GPIO_Pin_9)  //数据端口	PA0 
#define DS18B20_DQ_OUT_High GPIO_SetBits(GPIOG,GPIO_Pin_9)  //数据端口	PA0 
#define DS18B20_DQ_IN GPIO_ReadInputDataBit(GPIOG,GPIO_Pin_9)   //数据端口	PA0

void DS18B20_GPIO_Config(void);
void DS18B20_Mode_IPU(void);
void DS18B20_Mode_Out(void);
u8 DS18B20_Init(void);//初始化DS18B20
short DS18B20_Get_Temp(void);//获取温度

void DS18B20_Write_Byte(u8 dat);//写入一个字节
u8 DS18B20_Read_Byte(void);//读出一个字节
u8 DS18B20_Read_Bit(void);//读出一个位
u8 DS18B20_Answer_Check(void);//检测是否存在DS18B20
void DS18B20_Rst(void);//复位DS18B20    
#endif















