#ifndef __RTC_H
#define	__RTC_H


#include "stm32f10x.h"
#include "stm32f10x_bkp.h"
#include "stm32f10x_rtc.h"
#include "stm32f10x_pwr.h"
#include <time.h>
#include <stdio.h>
#include <string.h>


typedef struct tm NORMALTIME;//时间类型；把不是很友好的西方时间表示形式，转换为我们习惯的表示方式
                      //如：1-12月的表示由原来的0-11改为1-12，0表示星期天改为7表示，国际时间改为东八区时间
typedef time_t    TIMESTAMP;//时间戳类型
void              RTC_Init(NORMALTIME t);
void              RTC_Rst(TIMESTAMP ts);//RTC复位
TIMESTAMP         Time_GetTimeStamp(void); //获取RTC中的时间戳
void              Time_SetTimeStamp(TIMESTAMP ts);//设置RTC中的时间
NORMALTIME        Time_GetTime(void);
void              Time_SetTime(NORMALTIME t);

static void NVIC_Configuration_RTC(void); //RTC模块的中断配置
static void RTC_Configuration(void);
#endif /* __XXX_H */
