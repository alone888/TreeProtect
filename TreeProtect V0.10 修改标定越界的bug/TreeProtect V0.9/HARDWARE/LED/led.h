#ifndef __LED_H
#define __LED_H	 
#include "sys.h"
//////////////////////////////////////////////////////////////////////////////////	 
//������ֻ��ѧϰʹ�ã�δ��������ɣ��������������κ���;
//ALIENTEKս��STM32������
//LED��������	   
//����ԭ��@ALIENTEK
//������̳:www.openedv.com
//�޸�����:2012/9/2
//�汾��V1.0
//��Ȩ���У�����ؾ���
//Copyright(C) ������������ӿƼ����޹�˾ 2009-2019
//All rights reserved									  
////////////////////////////////////////////////////////////////////////////////// 
#define LED0 PEout(5)// PE5
#define LED1 PEout(6)// PE6	

#define BLED1 PFout(1)// PE6
#define BLED2 PFout(3)// PE6
#define BLED3 PFout(5)// PE6

#define IS_BUT_DN GPIO_ReadInputDataBit(GPIOF,GPIO_Pin_7)//���㰴��



#define BEEP PBout(8)// PB8

void LED_Init(void);//��ʼ��

		 				    
#endif
