#include "stm32f10x.h"
#include "dataflash.h"

#define FLASH_START_ADDR 	(0x8010000)

extern s16 IndCalib[5][21];
void wr_IndCalib_to_flash(void);
void rd_IndCalib_from_flash(void);
s16 Volt2Distance(u8 IndID,s16 v);
