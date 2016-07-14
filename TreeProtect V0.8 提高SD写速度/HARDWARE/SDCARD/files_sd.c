//����ļ��ǽ�����SD�����ڵ�����֮�ϣ���Ҫ���������Ŀ������ 
#include "sd_card.h"
#include "usart.h"
#include "rtc.h"

uchar F_wait_response = 0;
uchar SD_cmd_return_byte = 0;

//u Bit 0��SD ��������״̬��1 ��ʾSD �����ڣ�0 ��ʾSD �������ڣ���ע�����������ͬ��
//u Bit 1��SD ��д����״̬��1 ��ʾSD ��д������0��ʾ������д������
//u Bit 2��SD ����ʼ���ɹ�״̬��1 ��ʾSD����ʼ���ɹ���0 ��ʾSD����ʼ�����ɹ�
//u Bit 3���ļ�ϵͳ���ͣ�1 ��ʾ��FAT�ļ�ϵͳ��0 ��ʾ��FAT32 �ļ�ϵͳ����ע�����������ͬ��
//u Bit 4���ļ���״̬��1 ��ʾ��ǰ���ļ��򿪣�0 ��ʾ����û���ļ��򿪣�
//u Bit 5���޶��壻
//u Bit 6��ϵͳæ״̬��1 ��ʾϵͳ������æ״̬��0 ��ʾϵͳ���У�
//u Bit 7��У���״̬��1 ��ʾ���������У��Ͳ���ȷ��
	
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


//����0		����
//����1		��ȡϵͳ״̬ δ��Ӧ
//����2		�����ļ�
//����3		���ļ�
//����4		д������
//����5		��������
//����6		�ر��ļ�
//����7		SD����busy״̬


/**********************************************************************************/
char write_string_to_files(uchar * data)
{
	int waitCnt=0;
	int maxWaitCnt = 100000;
	int strlength = 0;

	NORMALTIME time;
	uchar fileName[30];//����ֻ��13���ȣ���Խ����
	time = Time_GetTime();
	sprintf(fileName,"%02d%02d%02d%02d.csv",time.tm_year-2000,time.tm_mon,time.tm_mday,time.tm_hour);
	
	
	//�ٻ�ȡϵͳ״̬  
	//----------------------------------------------------------------------------
	waitCnt=0;
	while(IS_SD_BUSY)
	{
		waitCnt++;
		if(waitCnt>maxWaitCnt)
		{
			return 7;
		}
	};//���ж�SD�Ƿ�æ

	SD_read_system();//�ϵ�󴮿ڵ�һ���ֽڷ�����ȥ
	SD_read_system();
	//�ȴ�״̬
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
	if((SD_cmd_return_byte & 0xCF) == 0x0D)//û���ļ���
	{
	}
	else if((SD_cmd_return_byte & 0xCF) == 0x1D)//�Ѿ����ļ��� ��ر�
	{
		SD_close_file();
		while(IS_SD_BUSY);
	}
	else
	{
		return SD_cmd_return_byte;
	}

	//�ڴ����ļ�
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
	
	//�۴��ļ�
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
	
	//��д���ļ�
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
	
	//�ݱ����ļ�
	//---------------------------------------------------------------------------- 
	SD_save_file();//�����ļ�
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

	//�޹ر��ļ�
	//---------------------------------------------------------------------------- 
	SD_close_file();//�ر��ļ�
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





