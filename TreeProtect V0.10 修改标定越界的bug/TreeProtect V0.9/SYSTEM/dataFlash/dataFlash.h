//-------------------DataFlash.h----------------------------------

#ifndef   __DATAFLASH_H__
#define   __DATAFLASH_H__


#include "dataType.h"
#include "stm32f10x_flash.h"



#if defined (STM32F10X_HD) || defined (STM32F10X_HD_VL) || defined (STM32F10X_CL) || defined (STM32F10X_XL)
  //#define FLASH_PAGE_SIZE    ((uint16_t)0x800)
  #define FLASH_PAGE_SIZE    ((uint16_t)0x400)
#else
  #define FLASH_PAGE_SIZE    ((uint16_t)0x400)
#endif

//extern uint32_t g_FlashAddr;//STM32的地址是32位的
//extern const uint8_t imageBuffer[1024];
//
//extern void allocatFlashStartAddr(void);


int Flash_Read(uint32_t iAddress, uint8_t *buf, int32_t iNbrToRead) ;
int Flash_Write(uint32_t iAddress, uint8_t *buf, uint32_t iNbrToWrite);


#endif

