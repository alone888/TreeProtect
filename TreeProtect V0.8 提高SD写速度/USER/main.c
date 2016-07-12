#include "led.h"
#include "delay.h"
#include "key.h"
#include "sys.h"
#include "usart.h"
#include "timer.h"
#include "rtc.h"
#include "sd_card.h"
#include "ds18b20.h"
#include "calib.h"
#include "crc16.h"
/************************************************************/
//更新说明：




/************************************************************/
//5个串口  
short g_IndVal[4];//±5mm  ±5000  以um为单位
int g_Pa; //先以电压表示 不标定。

short g_IndValOffset[4];
int g_PaOffset;

short g_WindSpeed;
short g_WindDir;
int g_Temper;
char g_weightCalib[4];

/*******************************************************************************/	  
//                 |起始寄存器|	4个寄存器 4路AD| crc校验 低位在前|
// 读AD 发送 01 03   00 68       00 04			  C5 D5
//              |0x30 为第9路AD| 

//                | 收到8字节 |	第9路 |第10路|第11路|第12路| 校验
// 收到      01 03      08 	    XX XX  XX XX  XX XX  XX XX	 XX XX
/*******************************************************************************/
void sendADCmd()
{
	send_byte_to_usart5(0x01);
	send_byte_to_usart5(0x03);
	send_byte_to_usart5(0x00);
	send_byte_to_usart5(0x68);
	send_byte_to_usart5(0x00);
	send_byte_to_usart5(0x04);
	send_byte_to_usart5(0xC5);
	send_byte_to_usart5(0xD5);
}

/*******************************************************************************/	  
//                 |起始寄存器|	2个寄存器 1路重量| crc校验 低位在前|
// 读称重 发送 01 03   00 00       00 02			  C4 0B

//                | 收到4字节 |	重量低16位 |重量高16位| 校验
// 收到      01 03      08 	      XX XX       XX XX     XX XX
/*******************************************************************************/
void sendWeightCmd()
{
	send_byte_to_usart4(0x01);
	send_byte_to_usart4(0x03);
	send_byte_to_usart4(0x00);
	send_byte_to_usart4(0x00);
	send_byte_to_usart4(0x00);
	send_byte_to_usart4(0x02);
	send_byte_to_usart4(0xC4);
	send_byte_to_usart4(0x0B);
}
/*******************************************************************************/	  
// 给称重传感器发清零标定

/*******************************************************************************/
void sendWeightZeroCmd()
{
	send_byte_to_usart4(0x01);
	send_byte_to_usart4(0x06);
	send_byte_to_usart4(0x00);
	send_byte_to_usart4(0x12);
	send_byte_to_usart4(0x00);
	send_byte_to_usart4(0x01);
	send_byte_to_usart4(0xE8);
	send_byte_to_usart4(0x0F);
}
/*******************************************************************************/	  
// 给称重传感器发载重标定

/*******************************************************************************/
void sendWeightLoad1Cmd()
{
	send_byte_to_usart4(0x01);
	send_byte_to_usart4(0x06);
	send_byte_to_usart4(0x00);
	send_byte_to_usart4(0x12);
	send_byte_to_usart4(0x00);
	send_byte_to_usart4(0x02);
	send_byte_to_usart4(0xA8);
	send_byte_to_usart4(0x0E);
}
void sendWeightLoad2Cmd(char first,char second)
{
	char WCmdStr[20];
	int crc16 = 0;
	char crcFirst=0,crcAfter = 0;
	//0110000800020427100000F978  发送给称重模块的标定数据
	WCmdStr[0] = 0x01;
	WCmdStr[1] = 0x10;
	WCmdStr[2] = 0x00;
	WCmdStr[3] = 0x08;
	WCmdStr[4] = 0x00;
	WCmdStr[5] = 0x02;
	WCmdStr[6] = 0x04;
	WCmdStr[7] = first;
	WCmdStr[8] = second;
	WCmdStr[9] = 0x00;
	WCmdStr[10] = 0x00;
	crc16 = getCRC16_LowFirst(WCmdStr,11);
	crcFirst = (crc16 & (0xFF<<8))>>8;
	crcAfter = 	crc16;
	WCmdStr[11] = crcFirst;
	WCmdStr[12] = crcAfter;  
	send_buf_to_usart4(WCmdStr,13);
}



/*******************************************************************************/	  
//风速地址1  串口2

//                 |起始寄存器|	1个寄存器 1路风速| crc校验 低位在前|
// 读风速风向 发送 01 03   00 00       00 01			  84 0A

//                | 收到2字节 |	路风速 | 校验				十六进制  十进制  实际值
// 收到      01 03      02 	      TH TL  XX XX				0x0026      38    3.8m/s
/*******************************************************************************/
void sendWindSpeedCmd()
{
	send_byte_to_usart2(0x01);
	send_byte_to_usart2(0x03);
	send_byte_to_usart2(0x00);
	send_byte_to_usart2(0x00);
	send_byte_to_usart2(0x00);
	send_byte_to_usart2(0x01);
	send_byte_to_usart2(0x84);
	send_byte_to_usart2(0x0A);

//不论地址多少都应该通的命令
//	send_byte_to_usart2(0x00);
//	send_byte_to_usart2(0x10);
//	send_byte_to_usart2(0x01);
//	send_byte_to_usart2(0x01);
//	send_byte_to_usart2(0xC1);
//	send_byte_to_usart2(0xB1);

}
/*******************************************************************************/	  
//风速地址1  风向地址2

//                 |起始寄存器|	1个寄存器 1路风速| crc校验 低位在前|
// 读风向发送 02 03   00 00       00 01			84  39

//                | 收到2字节 |	路风速 | 校验				十六进制  十进制  实际值
// 收到       02 03    02 	     TH TL   XX XX				0x0167      359    359度
/*******************************************************************************/
void sendWindDirCmd()
{

	send_byte_to_usart3(0x01);
	send_byte_to_usart3(0x03);
	send_byte_to_usart3(0x00);
	send_byte_to_usart3(0x00);
	send_byte_to_usart3(0x00);
	send_byte_to_usart3(0x01);
	send_byte_to_usart3(0x84);
	send_byte_to_usart3(0x0A);

//	send_byte_to_usart3(0x00);
//	send_byte_to_usart3(0x10);
//	send_byte_to_usart3(0x01);
//	send_byte_to_usart3(0x01);
//	send_byte_to_usart3(0xC1);
//	send_byte_to_usart3(0xB1);
}

					   
short Temp;
char lastSDerr;//SD卡上次的错误
char windSpeedDirFlag = 0;
char g_uart3_used_for_SD=0;	 
char CmdStr[200];
int dateSendtoPC[12];
int main(void)
{		

	u16 times=0,i=0; 

	NORMALTIME cur_time; //设置RTC的时间用
	
	//Flash_Write(0x08041000,(u8 *)IndCalib,sizeof(IndCalib));

	Flash_Read(0x08041000,(u8 *)IndCalib,sizeof(IndCalib));



	delay_init();	    	 //延时函数初始化	  
  	NVIC_Configuration(); 	 //设置NVIC中断分组2:2位抢占优先级，2位响应优先级

	uart1_init(115200);	 //上位机通信模块
	uart2_init(9600);	 //风速传感器模块 A2 A3 要转为232
	//uart3_init(19200);	 //SD卡模块	（兼职风向-因为3有重映射功能）
	uart4_init(9600);	 //称重模块
	uart5_init(9600);	 //AD模块

 	LED_Init();			 //LED端口初始化
	KEY_Init();          //初始化与按键连接的硬件接口
	SD_init();

	OPEN_SD_POWER;
	CLOSE_SD_POWER;
	OPEN_SD_POWER;
	CLOSE_SD_POWER;
	OPEN_SD_POWER;

	cur_time.tm_year = 2016; //2016-1900
	cur_time.tm_mon = 5;
	cur_time.tm_mday = 29;
	cur_time.tm_hour = 23;
	cur_time.tm_min = 56;
	cur_time.tm_sec = 2;

    RTC_Init(cur_time);//

	while(DS18B20_Init())//初始化DS18B20,兼检测18B20
	{
		printf("DS18B20 Check Failed!"); 
		printf("Please Check!  ");   
	}
	printf("DS18B20 Ready!     ");

 	while(1)
	{		

		if(IS_BUT_DN) 
		{
			BEEP = 0;
		}
		else
		{
			BEEP = 1;
			g_PaOffset= -g_Pa;

			g_IndValOffset[0]= -Volt2Distance(0,g_IndVal[0]);
			g_IndValOffset[1]= -Volt2Distance(1,g_IndVal[1]);
			g_IndValOffset[2]= -Volt2Distance(2,g_IndVal[2]);
			g_IndValOffset[3]= -Volt2Distance(3,g_IndVal[3]);
		}
		
		if(times%60==0) 
		{
			windSpeedDirFlag++;
			sendWindSpeedCmd(); //串口2 带232
			//printf("3\r\n");
			if(5 == windSpeedDirFlag)
			{
				windSpeedDirFlag = 0; 	
			}
			//printf("4\r\n");
			//风速的反应慢 一秒钟最多发一次指令 不然传感器要疯掉
			if(0 == windSpeedDirFlag)
			{
				uart3_init(19200);	 //SD卡模块	（兼职风向-因为3有重映射功能）
				g_uart3_used_for_SD = 1;
								//				日期			时间		  位移1, 2,   3,  4	 |拉力|                    温度 |SD|风速|风向
				sprintf(CmdStr,"#01,%02d-%02d-%02d,%02d:%02d:%02d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%1d,%04d,%04d!\r\n",
												cur_time.tm_year,cur_time.tm_mon,cur_time.tm_mday,
												cur_time.tm_hour,cur_time.tm_min,cur_time.tm_sec,
												dateSendtoPC[0],dateSendtoPC[1],dateSendtoPC[2],dateSendtoPC[3],
												dateSendtoPC[4],
												dateSendtoPC[6],dateSendtoPC[7],dateSendtoPC[8],dateSendtoPC[9],
												g_Temper,lastSDerr,g_WindSpeed,g_WindDir);
				//printf("7\r\n");
				lastSDerr = write_string_to_files(CmdStr);
			}
			else
			{
				//printf("8\r\n");
				uart3_init2(9600);
				//SD卡的usart3 重映射到 PB10 和PB11口 来读风向
				g_uart3_used_for_SD = 0;
				send_byte_to_usart3(0x02);//初始化完后第一个字节会发不成功
				sendWindDirCmd();//串口3
			}
			//printf("11\r\n");
			cur_time = Time_GetTime();
		}  

		if(times%10==0) 
		{			
			//sendWeightLoad1Cmd();
			//称重标定代码
			//printf("12\r\n");
			switch(PC_Wcmd)
			{
				case 0x30:
					//printf("13\r\n");
					BEEP = 1;
					sendWeightZeroCmd();
					BEEP = 0;
					PC_Wcmd = 0;
				break;
	
				case 0x40:
					//printf("14\r\n");
					BEEP = 1;
					sendWeightLoad1Cmd();					
					PC_Wcmd = 0;
					BEEP = 0;
				break;
				case 0x41:
					//printf("15\r\n");
					BEEP = 1;
					sendWeightLoad2Cmd(g_weightCalib[0],g_weightCalib[1]);
					PC_Wcmd = 0;
					BEEP = 0;
				break;
				default:
					//printf("16\r\n");
					//sendWeightLoad2Cmd(0x27,0x10);
					sendWeightCmd(); //串口4
				break;
			}

			BLED3=1;
			sendADCmd(); //串口5
			BLED3=0;
			dateSendtoPC[0] = g_IndVal[0];
			dateSendtoPC[1] = g_IndVal[1];
			dateSendtoPC[2] = g_IndVal[2];
			dateSendtoPC[3] = g_IndVal[3];
			dateSendtoPC[4] = g_PaOffset + g_Pa;

			dateSendtoPC[6] = g_IndValOffset[0] + Volt2Distance(0,g_IndVal[0]);
			dateSendtoPC[7] = g_IndValOffset[1] + Volt2Distance(1,g_IndVal[1]);
			dateSendtoPC[8] = g_IndValOffset[2] + Volt2Distance(2,g_IndVal[2]);
			dateSendtoPC[9] = g_IndValOffset[3] + Volt2Distance(3,g_IndVal[3]);
		}
		switch(PC_cmd)
		{
			case 1:
				BLED2 = 1;
					//				日期			时间		  位移1, 2,   3,  4	 |拉力|                    温度 |SD|风速|风向
				sprintf(CmdStr,"#01,%02d-%02d-%02d,%02d:%02d:%02d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%04d,%d,%04d,%04d!\r\n",
												cur_time.tm_year,cur_time.tm_mon,cur_time.tm_mday,
												cur_time.tm_hour,cur_time.tm_min,cur_time.tm_sec,
												dateSendtoPC[0],dateSendtoPC[1],dateSendtoPC[2],dateSendtoPC[3],
												dateSendtoPC[4],
												dateSendtoPC[6],dateSendtoPC[7],dateSendtoPC[8],dateSendtoPC[9],
												g_Temper,lastSDerr,g_WindSpeed,g_WindDir);
				send_string_to_usart1(CmdStr);
				BLED2 = 0;
				PC_cmd = 0;
			break;
			case 0x10:
			case 0x11:
			case 0x12:
			case 0x13:
			case 0x14:
				BEEP = 1;
				sprintf(CmdStr,"#%02x",PC_cmd);
				for(i=0;i<21;i++)
				{
					sprintf(CmdStr,"%s,%+05d",CmdStr,IndCalib[PC_cmd-0x10][i]);	
				}
				sprintf(CmdStr,"%s!\r\n",CmdStr);
				send_string_to_usart1(CmdStr);
				PC_cmd = 0;
				BEEP = 0;
			break;
			case 0x20:
			case 0x21:
			case 0x22:
			case 0x23:
			case 0x24:
				sprintf(CmdStr,"#%02x",PC_cmd-0x10);
				for(i=0;i<21;i++)
				{
					sprintf(CmdStr,"%s,%+05d",CmdStr,IndCalib[PC_cmd-0x20][i]);	
				}
				sprintf(CmdStr,"%s!\r\n",CmdStr);
				send_string_to_usart1(CmdStr);
				PC_cmd = 0;

			break; 		
		}
		times++;  
		delay_ms(10);  
	}	 
 }

