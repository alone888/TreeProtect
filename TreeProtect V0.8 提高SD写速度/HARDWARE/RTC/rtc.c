//�����е�ʱ�䣨normal time���Ǹ��ݹ��ʱ�׼ʱ���ת��Ϊ�������ı���ʱ�䣨NORMALTIME����
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#include "rtc.h"

/*******************************************************************************
* Function Name  : RTC_Init
* Description    : RTC Initialization
* Call Function  : NVIC_Configuration_RTC,RTC_Configuration
*******************************************************************************/
void RTC_Init(NORMALTIME t)//RTC��ʼ��
{
  NVIC_Configuration_RTC();
  //��������������
  if (BKP_ReadBackupRegister(BKP_DR1) != 0xA5A5)//�ж�RTCģ���Ƿ����������
  {
    RTC_Configuration();//���û�����ã��������Ӧ�ĳ�ʼ����������
 	Time_SetTime(t);//д�������RTCʱ��
 	/* Adjust time by values entred by the user on the hyperterminal */
    BKP_WriteBackupRegister(BKP_DR1, 0xA5A5);//��0xA5A5д��BKP_DR1�Ĵ�����
  }
  return;
}
/*******************************************************************************
* Function Name  : NVIC_Configuration_RTC
* Description    : Configures the nested vectored interrupt controller.
* Input          : None
* Output         : None
* Return         : None
* Attention : None
*******************************************************************************/
static void NVIC_Configuration_RTC(void) //RTCģ����ж�����
 {
   NVIC_InitTypeDef NVIC_InitStructure;
   /* Configure one bit for preemption priority */
   NVIC_PriorityGroupConfig(NVIC_PriorityGroup_1);
   /* Enable the RTC Interrupt */
   NVIC_InitStructure.NVIC_IRQChannel = RTC_IRQn;
   NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 1;
   NVIC_InitStructure.NVIC_IRQChannelSubPriority = 0;
   NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
   NVIC_Init(&NVIC_InitStructure);
 }
/*******************************************************************************
* Function Name  : RTC_Configuration
* Description    : Configures the RTC.
* Input          : None
* Output         : None
* Return         : None
* Attention : None
*******************************************************************************/
static void RTC_Configuration(void)//RTC�ĳ�ʼ���Ļ������ã�����RTC��ʱ��Դѡ��ͷ�Ƶ�������жϵ�ʹ�ܣ���ΪRTC_Init(void)��׼��
{
  /* Enable PWR and BKP clocks */
  RCC_APB1PeriphClockCmd(RCC_APB1Periph_PWR | RCC_APB1Periph_BKP, ENABLE);//����PWR��BKP��ʱ��
  /* Allow access to BKP Domain */
  PWR_BackupAccessCmd(ENABLE);//�������BKP����PWR_CR�е�DBPλ����д1���������BKP��RTCģ��Ĳ���
  /* Reset Backup Domain */
  BKP_DeInit();//��λBKP
  /* Enable LSE */
  RCC_LSEConfig(RCC_LSE_ON); //�ⲿ����ʱ��ʹ��
  /* Wait till LSE is ready */
  while (RCC_GetFlagStatus(RCC_FLAG_LSERDY) == RESET){}//�ȴ�ʱ��׼����  
  /* Select LSE as RTC Clock Source */
  RCC_RTCCLKConfig(RCC_RTCCLKSource_LSE); //ѡ���ⲿ����ʱ����RTC��ʱ��Դ
  /* Enable RTC Clock */
  RCC_RTCCLKCmd(ENABLE); //ʹ��RTC��ʱ�� ����ͨ����RCC_BDCR�е�RTCENλ�Ĳ���������RTC��ʱ�ӵ�
  /* Wait for RTC registers synchronization */
  RTC_WaitForSynchro();
  /* Wait until last write operation on RTC registers has finished */
  RTC_WaitForLastTask();
  /* Enable the RTC Second */
  //RTC_ITConfig(RTC_IT_SEC, ENABLE);//ʹ��RTC�����ж�
  /* Wait until last write operation on RTC registers has finished */
  RTC_WaitForLastTask();
  /* Set RTC prescaler: set RTC period to 1sec */
  RTC_SetPrescaler(32767); /* RTC period = RTCCLK/RTC_PR = (32.768 KHz)/(32767+1) *///RTCΪ32767+1��Ƶ��ÿ��һ������ж�
  /* Wait until last write operation on RTC registers has finished */
  RTC_WaitForLastTask();
}


///////////////////ʱ����ʽ��ת��/////////////////////////////////
TIMESTAMP TimeConv_Nomal2Stamp(NORMALTIME t)//������������ʱ��ת��Ϊʱ���
{
	t.tm_year-=1900;//��Ȼʱ�������1970�꣨��������0x00000000����ʼ��ʱ����ʱ��ת����ת��1900�꣨������32bit�����������ʱ��
	t.tm_mon -=1;
	if(t.tm_wday>=7)
	t.tm_wday=0;
	t.tm_hour-=0;

	return mktime(&t);
}
NORMALTIME TimeConv_Stamp2Nomal(TIMESTAMP ts)//ʱ���ת��Ϊ������������ʱ��
{
	NORMALTIME *pt,t;
	pt = localtime(&ts);//����Ϊ��ʱ���ת���ɴ�1900��ʼ�����ʱ��ֵ����Ҫ���Ͽ�ʼ��ʱ��
	t=*pt;
	t.tm_year+=1900;
	t.tm_mon +=1;
	if(t.tm_wday==0)
	t.tm_wday=7;
	t.tm_hour+=0;//תΪ����ʱ��	������תΪ����ʱ����  ��̫ţ����������������������� ��ת�� ת�˶�����25����
	return t;
}
//////////////////ʱ�����ʽ��ȡʱ�������ʱ��///////////////////////
TIMESTAMP Time_GetTimeStamp(void) //��ȡRTC�е�ʱ���
{
	return (TIMESTAMP)RTC_GetCounter();
}

void Time_SetTimeStamp(TIMESTAMP ts)//����RTC�е�ʱ�䣨ʱ�������ʽ��
{
	RTC_Configuration(); 
	RTC_WaitForLastTask();
	RTC_SetCounter((u32)ts);
	BKP_WriteBackupRegister(BKP_DR1, 0xA5A5);//��0xA5A5д��BKP_DR1�Ĵ����� 
	RTC_WaitForLastTask();
}

/////////////////����ʱ�䷽ʽ��ȡ������ʱ��/////////////////////////
NORMALTIME Time_GetTime(void) //��ȡ������������ʱ��
{
  return TimeConv_Stamp2Nomal((TIMESTAMP)RTC_GetCounter());
}

void Time_SetTime(NORMALTIME t)//����������������ʱ��
{
	Time_SetTimeStamp(TimeConv_Nomal2Stamp(t));
}

//////////////////RTCģ�鸴λ//////////////////////////////////////
void RTC_Rst(TIMESTAMP ts)//RTC��λ
{
	RTC_Configuration();//���û�����ã��������Ӧ�ĳ�ʼ����������
	Time_SetTimeStamp(ts);
	/* Adjust time by values entred by the user on the hyperterminal */
	BKP_WriteBackupRegister(BKP_DR1, 0x0000);//��0x0000д��BKP_DR1�Ĵ�����
}
//   RTC_EnterConfigMode();
//   /* Set RTC COUNTER MSB word */
//   RTC->CNTH = CounterValue >> 16;
//   /* Set RTC COUNTER LSB word */
//   RTC->CNTL = (CounterValue & RTC_LSB_MASK);
//   RTC_ExitConfigMode();


// /*******************************************************************************
// * Function Name  : Time_GetCalendarTime
// * Description    : ��RTCȡ��ǰʱ�������ʱ�䣨struct tm��
// * Input          : None
// * Output         : None
// * Return         : struct tm
// * Attention : None
// *******************************************************************************/
// struct tm Time_GetCalendarTime(void) //��ȡRTC�е�����ʱ��
// {
// time_t t_t;
// struct tm t_tm;

// t_t = (time_t)RTC_GetCounter();
// t_tm = Time_ConvUnixToCalendar(t_t);
// return t_tm;
// }

/******************************************************************************************************************************
���º���Ϊ����RTCʱ��
******************************************************************************************************************************/
// /*******************************************************************************
// * Function Name  : Time_SetCalendarTime
// * Description    : ��������Calendar��ʽʱ��ת����UNIXʱ���д��RTC
// * Input          : - t: struct tm
// * Output         : None
// * Return         : None
// * Attention : None
// *******************************************************************************/
// void Time_SetCalendarTime(struct tm t)//����������ʽ����RTC�е�ʱ��
// {
// Time_SetUnixTime(Time_ConvCalendarToUnix(t));
// return;
// }

// /******************************************************************************************************************************
// ������������Ϊʱ���������ʱ��֮���ת��������ʱ��Ϊ��1900-1-1��ʼ�ģ�
// ******************************************************************************************************************************/
// /*******************************************************************************
// * Function Name  : Time_ConvUnixToCalendar
// * Description    : ת��UNIXʱ���Ϊ����ʱ��
// * Input          : - t: ��ǰʱ���UNIXʱ���
// * Output         : None
// * Return         : struct tm
// * Attention : None
// *******************************************************************************/
// struct tm Time_ConvUnixToCalendar(time_t t)//��ʱ���ת��Ϊ����ʱ��(ǰ����ʱ�����ʼʱ���������ʼ�����ʱ����ͬ�����Ǵ�1900�꿪ʼ����)
// {
// struct tm *t_tm;
// t_tm = localtime(&t);//t_tm = localtime(&t);����Ϊ��ʱ���ת���ɴ�1900��ʼ�����ʱ��ֵ����Ҫ���Ͽ�ʼ��ʱ��
// t_tm->tm_year += 1900;/* localtimeת�������tm_year�����ֵ����Ҫת�ɾ���ֵ */
//
// return *t_tm;
// }

// /*******************************************************************************
// * Function Name  : Time_ConvCalendarToUnix
// * Description    : д��RTCʱ�ӵ�ǰʱ��
// * Input          : - t: struct tm
// * Output         : None
// * Return         : time_t
// * Attention : None
// *******************************************************************************/
// time_t Time_ConvCalendarToUnix(struct tm t)//������ʱ��ת��Ϊʱ���
// {
// t.tm_year -= 1900;  /* �ⲿtm�ṹ��洢�����Ϊ2008��ʽ*/
// /* ��time.h�ж������ݸ�ʽΪ1900�꿪ʼ����� */
// /* ���ԣ�������ת��ʱҪ���ǵ�������ء�*/
// return mktime(&t);
// }
 
// /*******************************************************************************
// * Function Name  : Time_Regulate
// * Description    : None
// * Input          : None
// * Output         : None
// * Return         : None
// * Attention : None
// *******************************************************************************/
// void Time_Regulate(void)
// {
//   struct tm time;
//     memset(&time, 0 , sizeof(time) );/* ��սṹ�� */
//     
//     time.tm_year =2012;//�趨��1970-2037
//   
//     time.tm_mon=1-1;//�趨��(1-12)-1

//     time.tm_mday = 1;//�趨��1-31

//     time.tm_hour =1;//�趨Сʱ0-23

//     time.tm_min = 1;//�趨��0-59

//     time.tm_sec =1;//�趨��0-59

//   /* Return the value to store in RTC counter register */
//   Time_SetCalendarTime(time);//���������趨��RTC��������  
// }
// /*******************************************************************************
// * ���ļ�ʵ�ֻ���RTC�����ڹ��ܣ��ṩ�����յĶ�д��������ANSI-C��time.h��
// * 
// * RTC�б����ʱ���ʽ����UNIXʱ�����ʽ�ġ���һ��32bit��time_t������ʵΪu32��
// *
// * ANSI-C�ı�׼���У��ṩ�����ֱ�ʾʱ�������  �ͣ�
// * time_t:   UNIXʱ�������19xx-1-1��ĳʱ�侭����������
// * typedef unsigned int time_t;
// * 
// * struct tm:Calendar��ʽ����������ʽ��
// *   tm�ṹ���£�
// *   struct tm {
// *   int tm_sec;   // �� seconds after the minute, 0 to 60(0 - 60 allows for the occasional leap second)
// *   int tm_min;   // �� minutes after the hour,   0 to 59
// *  int tm_hour;  // ʱ hours since midnight, 0 to 23
// *  int tm_mday;  // �� day of the month, 1 to 31
// *  int tm_mon;   // �� months since January, 0 to 11
// *  int tm_year;  // �� years since 1900
// *  int tm_wday;  // ���� days since Sunday, 0 to 6
// *  int tm_yday;  // ��Ԫ��������� days since January 1, 0 to 365
// * int tm_isdst; // ����ʱ����Daylight Savings Time flag
// * ...
// * }
// * ����wday��yday�����Զ����������ֱ�Ӷ�ȡ
// * mon��ȡֵΪ0-11
// ****ע��***��
// *tm_year:��time.h���ж���Ϊ1900�������ݣ���2008��Ӧ��ʾΪ2008-1900=108
// * ���ֱ�ʾ�������û���˵����ʮ���Ѻã�����ʵ�нϴ���졣
// * �����ڱ��ļ��У����������ֲ��졣
// * ���ⲿ���ñ��ļ��ĺ���ʱ��tm�ṹ�����͵����ڣ�tm_year��Ϊ2008
// * ע�⣺��Ҫ����ϵͳ��time.c�еĺ�������Ҫ���н�tm_year-=1900
// * 
// * ��Ա����˵����
// * struct tm Time_ConvUnixToCalendar(time_t t);
// * ����һ��Unixʱ�����time_t��������Calendar��ʽ����
// * time_t Time_ConvCalendarToUnix(struct tm t);
// * ����һ��Calendar��ʽ���ڣ�����Unixʱ�����time_t��
// * time_t Time_GetUnixTime(void);
// * ��RTCȡ��ǰʱ���Unixʱ���ֵ
// * struct tm Time_GetCalendarTime(void);
// * ��RTCȡ��ǰʱ�������ʱ��
// * void Time_SetUnixTime(time_t);
// * ����UNIXʱ�����ʽʱ�䣬����Ϊ��ǰRTCʱ��
// * void Time_SetCalendarTime(struct tm t);
// * ����Calendar��ʽʱ�䣬����Ϊ��ǰRTCʱ��
// * 
// * �ⲿ����ʵ����
// * ����һ��Calendar��ʽ�����ڱ�����
// * struct tm now;
// * now.tm_year = 2008;
// * now.tm_mon = 11;//12��
// * now.tm_mday = 20;
// * now.tm_hour = 20;
// * now.tm_min = 12;
// * now.tm_sec = 30;
// * 
// * ��ȡ��ǰ����ʱ�䣺
// * tm_now = Time_GetCalendarTime();
// * Ȼ�����ֱ�Ӷ�tm_now.tm_wday��ȡ������
// * 
// * ����ʱ�䣺
// * Step1. tm_now.xxx = xxxxxxxxx;
// * Step2. Time_SetCalendarTime(tm_now);
// * 
// * ��������ʱ��Ĳ�
// * struct tm t1,t2;
// * t1_t = Time_ConvCalendarToUnix(t1);
// * t2_t = Time_ConvCalendarToUnix(t2);
// * dt = t1_t - t2_t;
// * dt��������ʱ��������
// * dt_tm = mktime(dt);//ע��dt�����ƥ�䣬ansi���к���Ϊ�����ݣ�ע�ⳬ��
// * ����Բο�������ϣ�����ansi-c��ĸ�ʽ������ȹ��ܣ�ctime��strftime��
// * 
