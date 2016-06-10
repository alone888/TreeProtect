#include "led.h"

//////////////////////////////////////////////////////////////////////////////////	 
//本程序只供学习使用，未经作者许可，不得用于其它任何用途
//ALIENTEK战舰STM32开发板
//LED驱动代码	   
//正点原子@ALIENTEK
//技术论坛:www.openedv.com
//修改日期:2012/9/2
//版本：V1.0
//版权所有，盗版必究。
//Copyright(C) 广州市星翼电子科技有限公司 2009-2019
//All rights reserved									  
////////////////////////////////////////////////////////////////////////////////// 	   

//初始化PE5和PE6为输出口.并使能这两个口的时钟
//初始化PE4为输入口 		    
//LED IO初始化


void LED_Init(void)
{ 
	GPIO_InitTypeDef  GPIO_InitStructure;	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOB|RCC_APB2Periph_GPIOE|RCC_APB2Periph_GPIOF, ENABLE);	 //使能PB,PE端口时钟

/*********************输出口设置****************************************************************/	
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_6;				 //LED0-->PE.6  端口配置
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 		 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOE, &GPIO_InitStructure);					 //根据设定参数初始化GPIOB.5
	GPIO_SetBits(GPIOE,GPIO_Pin_6);							 //PE.6 输出高
	
	GPIO_ResetBits(GPIOE,GPIO_Pin_6);

	
	//GPIO_ResetBits(GPIOB,GPIO_Pin_5);
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_5;	    		 //LED1-->PE.5 端口配置, 推挽输出
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 		 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOE, &GPIO_InitStructure);	  				 //推挽输出 ，IO口速度为50MHz
	GPIO_SetBits(GPIOE,GPIO_Pin_5); 						 //PE.5 输出高 

	GPIO_ResetBits(GPIOE,GPIO_Pin_5);


/*********************面板上的三个LED灯和按键设置****************************************************************/
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_1;	    		 //LED1-->PF.5 端口配置, 推挽输出
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 		 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOF, &GPIO_InitStructure);	  				 //推挽输出 ，IO口速度为50MHz
	GPIO_SetBits(GPIOF,GPIO_Pin_1); 						 //PF.5 输出高 

	GPIO_ResetBits(GPIOF,GPIO_Pin_1);

	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_3;	    		 //LED2-->PF.5 端口配置, 推挽输出
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 		 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOF, &GPIO_InitStructure);	  				 //推挽输出 ，IO口速度为50MHz
	GPIO_SetBits(GPIOF,GPIO_Pin_3); 						 //PF.5 输出高 

	GPIO_ResetBits(GPIOF,GPIO_Pin_3);

	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_5;	    		 //LED3-->PF.5 端口配置, 推挽输出
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 		 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOF, &GPIO_InitStructure);	  				 //推挽输出 ，IO口速度为50MHz
	GPIO_SetBits(GPIOF,GPIO_Pin_5); 						 //PF.5 输出高 

	GPIO_ResetBits(GPIOF,GPIO_Pin_5);

	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_7;	    		 //按键-->PF.5 端口配置, 推挽输出
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU; 	 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOF, &GPIO_InitStructure);	  				 //按键输入 ，IO口速度为50MHz
//	GPIO_SetBits(GPIOF,GPIO_Pin_7); 						 //PF.7 输出高 
//
//	GPIO_ResetBits(GPIOF,GPIO_Pin_7);

/*********************蜂鸣器**********************************************************/
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8;	    		 //按键-->PF.5 端口配置, 推挽输出
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP; 	 //推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;		 //IO口速度为50MHz
	GPIO_Init(GPIOB, &GPIO_InitStructure);	  				 //按键输入 ，IO口速度为50MHz
	GPIO_SetBits(GPIOB,GPIO_Pin_8); 						 //PF.7 输出高 

	GPIO_ResetBits(GPIOB,GPIO_Pin_8);

}
 
