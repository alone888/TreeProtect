#ifndef __RTC_H
#define	__RTC_H


#include "stm32f10x.h"
#include "stm32f10x_bkp.h"
#include "stm32f10x_rtc.h"
#include "stm32f10x_pwr.h"
#include <time.h>
#include <stdio.h>
#include <string.h>


typedef struct tm NORMALTIME;//ʱ�����ͣ��Ѳ��Ǻ��Ѻõ�����ʱ���ʾ��ʽ��ת��Ϊ����ϰ�ߵı�ʾ��ʽ
                      //�磺1-12�µı�ʾ��ԭ����0-11��Ϊ1-12��0��ʾ�������Ϊ7��ʾ������ʱ���Ϊ������ʱ��
typedef time_t    TIMESTAMP;//ʱ�������
void              RTC_Init(NORMALTIME t);
void              RTC_Rst(TIMESTAMP ts);//RTC��λ
TIMESTAMP         Time_GetTimeStamp(void); //��ȡRTC�е�ʱ���
void              Time_SetTimeStamp(TIMESTAMP ts);//����RTC�е�ʱ��
NORMALTIME        Time_GetTime(void);
void              Time_SetTime(NORMALTIME t);

static void NVIC_Configuration_RTC(void); //RTCģ����ж�����
static void RTC_Configuration(void);
#endif /* __XXX_H */
