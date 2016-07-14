//这个文件是建立在SD卡串口的驱动之上，主要负责这个项目的驱动 
#include "sd_card.h"
#include "usart.h"
#include "rtc.h"

uchar F_wait_response = 0;
uchar SD_cmd_return_byte = 0;

//u Bit 0，SD 卡不存在状态，1 表示SD 卡存在，0 表示SD 不卡存在；（注意与其他命令不同）
//u Bit 1，SD 卡写保护状态，1 表示SD 卡写保护，0表示不处在写保护；
//u Bit 2，SD 卡初始化成功状态，1 表示SD卡初始化成功，0 表示SD卡初始化不成功
//u Bit 3，文件系统类型，1 表示是FAT文件系统，0 表示非FAT32 文件系统；（注意与其他命令不同）
//u Bit 4，文件打开状态，1 表示当前有文件打开，0 表示当期没有文件打开；
//u Bit 5，无定义；
//u Bit 6，系统忙状态，1 表示系统正处在忙状态，0 表示系统空闲；
//u Bit 7，校验和状态，1 表示发送命令的校验和不正确；
	
// 0 0 X X  1 1 0 1
// 
char IS_SD_OK()
{
	SD_read_system();

	F_wait_response = 0x01;
	SD_cmd_return_byte = 0;
	while(F_wait_response);
	if((SD_cmd_return_byte & 0xCD) == 0xCD)
	{
		return 0;
	}
	else
	{
		return SD_cmd_return_byte;
	}		  
}

/**********************************************************************************/
//#define SYS 0x01
//#define SD_CREATE 0x02
//#define SD_OPEN 0x06
//#define SD_WRITE 0x05
//#define SD_SAVE 0x04
//


//返回0		正常
//返回1		获取系统状态 未响应
//返回2		创建文件
//返回3		打开文件
//返回4		写入数据
//返回5		保存数据
//返回6		关闭文件
//返回7		SD处于busy状态


/**********************************************************************************/
char write_string_to_files(uchar * data)
{
	int waitCnt=0;
	int maxWaitCnt = 100000;
	int strlength = 0;

	NORMALTIME time;
	uchar fileName[30];//本来只有13长度，怕越界了
	time = Time_GetTime();
	sprintf(fileName,"%02d%02d%02d%02d.csv",time.tm_year-2000,time.tm_mon,time.tm_mday,time.tm_hour);
	
	
	//①获取系统状态  
	//----------------------------------------------------------------------------
	waitCnt=0;
	while(IS_SD_BUSY)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 7;
		}
	};//先判断SD是否忙

	SD_read_system();//上电后串口第一个字节发不出去
	SD_read_system();
	//等待状态
	F_wait_response = 0x01;
	SD_cmd_return_byte = 0;

	waitCnt = 0;
	while(F_wait_response)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 1;
		}
	};
	if((SD_cmd_return_byte & 0xCF) == 0x0D)//没有文件打开
	{
	}
	else if((SD_cmd_return_byte & 0xCF) == 0x1D)//已经有文件打开 则关闭
	{
		SD_close_file();
		while(IS_SD_BUSY);
	}
	else
	{
		return SD_cmd_return_byte;
	}

	//②创建文件
	//----------------------------------------------------------------------------
	SD_send_cmd(SD_CREATE,13,fileName);
	F_wait_response = 0x01;
	SD_cmd_return_byte = 0;
	
	waitCnt=0;
	while(F_wait_response)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 2;
		}
	};

	while(IS_SD_BUSY);
	
	//③打开文件
	//----------------------------------------------------------------------------
	SD_send_cmd(SD_OPEN,13,fileName);
	F_wait_response = 0x01;
	SD_cmd_return_byte = 0;

	waitCnt=0;
	while(F_wait_response)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 3;
		}
	};

	while(IS_SD_BUSY);
	
	//④写入文件
	//----------------------------------------------------------------------------
	strlength = strlen(data)+4;
	SD_send_data(SD_WRITE,strlen(data)+4,0xffffffff,data);
	F_wait_response = 0x01;
	SD_cmd_return_byte = 0;

	waitCnt=0;
	while(F_wait_response)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 4;
		}
	};

	while(IS_SD_BUSY);
	
	//⑤保存文件
	//---------------------------------------------------------------------------- 
	SD_save_file();//保存文件
	F_wait_response = 0x01;
	SD_cmd_return_byte = 0;
	
	waitCnt=0;
	while(F_wait_response)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 5;
		}
	};
	while(IS_SD_BUSY);

	//⑥关闭文件
	//---------------------------------------------------------------------------- 
	SD_close_file();//关闭文件
	F_wait_response = 0x01;
	SD_cmd_return_byte = 0;

	waitCnt=0;
	while(F_wait_response)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 6;
		}
	};
	while(IS_SD_BUSY);

	return 0;
}





