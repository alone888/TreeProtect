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
//����˵����




/************************************************************/
//5������  
short g_IndVal[4];//��5mm  ��5000  ��umΪ��λ
int g_Pa; //���Ե�ѹ��ʾ ���궨��

short g_IndValOffset[4];
int g_PaOffset;

short g_WindSpeed;
short g_WindDir;
int g_Temper;
char g_weightCalib[4];

/*******************************************************************************/	  
//                 |��ʼ�Ĵ���|	4���Ĵ��� 4·AD| crcУ�� ��λ��ǰ|
// ��AD ���� 01 03   00 68       00 04			  C5 D5
//              |0x30 Ϊ��9·AD| 

//                | �յ�8�ֽ� |	��9· |��10·|��11·|��12·| У��
// �յ�      01 03      08 	    XX XX  XX XX  XX XX  XX XX	 XX XX
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
//                 |��ʼ�Ĵ���|	2���Ĵ��� 1·����| crcУ�� ��λ��ǰ|
// ������ ���� 01 03   00 00       00 02			  C4 0B

//                | �յ�4�ֽ� |	������16λ |������16λ| У��
// �յ�      01 03      08 	      XX XX       XX XX     XX XX
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
// �����ش�����������궨

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
// �����ش����������ر궨

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
	//0110000800020427100000F978  ���͸�����ģ��ı궨����
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
//���ٵ�ַ1  ����2

//                 |��ʼ�Ĵ���|	1���Ĵ��� 1·����| crcУ�� ��λ��ǰ|
// �����ٷ��� ���� 01 03   00 00       00 01			  84 0A

//                | �յ�2�ֽ� |	·���� | У��				ʮ������  ʮ����  ʵ��ֵ
// �յ�      01 03      02 	      TH TL  XX XX				0x0026      38    3.8m/s
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

//���۵�ַ���ٶ�Ӧ��ͨ������
//	send_byte_to_usart2(0x00);
//	send_byte_to_usart2(0x10);
//	send_byte_to_usart2(0x01);
//	send_byte_to_usart2(0x01);
//	send_byte_to_usart2(0xC1);
//	send_byte_to_usart2(0xB1);

}
/*******************************************************************************/	  
//���ٵ�ַ1  �����ַ2

//                 |��ʼ�Ĵ���|	1���Ĵ��� 1·����| crcУ�� ��λ��ǰ|
// �������� 02 03   00 00       00 01			84  39

//                | �յ�2�ֽ� |	·���� | У��				ʮ������  ʮ����  ʵ��ֵ
// �յ�       02 03    02 	     TH TL   XX XX				0x0167      359    359��
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
char lastSDerr;//SD���ϴεĴ���
char windSpeedDirFlag = 0;
char g_uart3_used_for_SD=0;	 
char CmdStr[200];
int dateSendtoPC[12];
int main(void)
{		

	u16 times=0,i=0; 

	NORMALTIME cur_time; //����RTC��ʱ����
	
	//Flash_Write(0x08041000,(u8 *)IndCalib,sizeof(IndCalib));

	Flash_Read(0x08041000,(u8 *)IndCalib,sizeof(IndCalib));



	delay_init();	    	 //��ʱ������ʼ��	  
  	NVIC_Configuration(); 	 //����NVIC�жϷ���2:2λ��ռ���ȼ���2λ��Ӧ���ȼ�

	uart1_init(115200);	 //��λ��ͨ��ģ��
	uart2_init(9600);	 //���ٴ�����ģ�� A2 A3 ҪתΪ232
	//uart3_init(19200);	 //SD��ģ��	����ְ����-��Ϊ3����ӳ�书�ܣ�
	uart4_init(9600);	 //����ģ��
	uart5_init(9600);	 //ADģ��

 	LED_Init();			 //LED�˿ڳ�ʼ��
	KEY_Init();          //��ʼ���밴�����ӵ�Ӳ���ӿ�
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

	while(DS18B20_Init())//��ʼ��DS18B20,����18B20
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
			sendWindSpeedCmd(); //����2 ��232
			//printf("3\r\n");
			if(5 == windSpeedDirFlag)
			{
				windSpeedDirFlag = 0; 	
			}
			//printf("4\r\n");
			//���ٵķ�Ӧ�� һ������෢һ��ָ�� ��Ȼ������Ҫ���
			if(0 == windSpeedDirFlag)
			{
				uart3_init(19200);	 //SD��ģ��	����ְ����-��Ϊ3����ӳ�书�ܣ�
				g_uart3_used_for_SD = 1;
								//				����			ʱ��		  λ��1, 2,   3,  4	 |����|                    �¶� |SD|����|����
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
				//SD����usart3 ��ӳ�䵽 PB10 ��PB11�� ��������
				g_uart3_used_for_SD = 0;
				send_byte_to_usart3(0x02);//��ʼ������һ���ֽڻᷢ���ɹ�
				sendWindDirCmd();//����3
			}
			//printf("11\r\n");
			cur_time = Time_GetTime();
		}  

		if(times%10==0) 
		{			
			//sendWeightLoad1Cmd();
			//���ر궨����
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
					sendWeightCmd(); //����4
				break;
			}

			BLED3=1;
			sendADCmd(); //����5
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
					//				����			ʱ��		  λ��1, 2,   3,  4	 |����|                    �¶� |SD|����|����
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

