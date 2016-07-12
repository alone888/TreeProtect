//-------------------DataFlash.c----------------------------------


#include "dataFlash.h"



//void allocatFlashStartAddr(void)
//{
//	g_FlashAddr = (uint32_t) imageBuffer;//¼ÇÂ¼¿ÉÒÔÓÃflashÇøÓò
//}

uint16_t Flash_Write_Without_check(uint32_t iAddress, uint8_t *buf, uint16_t iNumByteToWrite) {
    uint16_t i;
    volatile FLASH_Status FLASHStatus = FLASH_COMPLETE;
    i = 0;
    
//    FLASH_UnlockBank1();
    while((i < iNumByteToWrite) && (FLASHStatus == FLASH_COMPLETE))
    {
      FLASHStatus = FLASH_ProgramHalfWord(iAddress, *(uint16_t*)buf);
      i = i+2;
      iAddress = iAddress + 2;
      buf = buf + 2;
    }
    
    return iNumByteToWrite;
}
/**
  * @brief  Programs a half word at a specified Option Byte Data address.
  * @note   This function can be used for all STM32F10x devices.
  * @param  Address: specifies the address to be programmed.
  * @param  buf: specifies the data to be programmed.
  * @param  iNbrToWrite: the number to write into flash
  * @retval if success return the number to write, -1 if error
  *  
  */
int Flash_Write(uint32_t iAddress, uint8_t *buf, uint32_t iNbrToWrite) {
                /* Unlock the Flash Bank1 Program Erase controller */
	uint32_t secpos;
	uint32_t iNumByteToWrite = iNbrToWrite;
	uint16_t secoff;
	uint16_t secremain;  
	uint16_t i = 0;    
	uint8_t tmp[FLASH_PAGE_SIZE];
	volatile FLASH_Status FLASHStatus;
	
	FLASH_Unlock();
	secpos=iAddress & (~(FLASH_PAGE_SIZE -1 )) ;//ÉÈÇøµØÖ· 
	secoff=iAddress & (FLASH_PAGE_SIZE -1);     //ÔÚÉÈÇøÄÚµÄÆ«ÒÆ
	secremain=FLASH_PAGE_SIZE-secoff;           //ÉÈÇøÊ£Óà¿Õ¼ä´óĞ¡ 
	FLASHStatus = FLASH_COMPLETE;
	
	if(iNumByteToWrite<=secremain) 
	secremain = iNumByteToWrite;//²»´óÓÚ4096¸ö×ÖÚ
        
    while( 1 ) {
		Flash_Read(secpos, tmp, FLASH_PAGE_SIZE);   //¶Á³öÕû¸öÉÈÇø
		for(i=0;i<secremain;i++) {       //Ğ£ÑéÊı¾İ
			if(tmp[secoff+i]!=0XFF)break;       //ĞèÒª²Á³ı 
		}
		if(i<secremain) {  //ĞèÒª²Á³ı
		    FLASHStatus = FLASH_ErasePage(secpos); //²Á³ıÕâ¸öÉÈÇø
		    if(FLASHStatus != FLASH_COMPLETE)
		      return -1;
		    for(i=0;i<secremain;i++) {   //¸´ÖÆ
		            tmp[i+secoff]=buf[i];   
		    }
		    Flash_Write_Without_check(secpos ,tmp ,FLASH_PAGE_SIZE);//Ğ´ÈëÕû¸öÉÈÇø  
		} else {
		    Flash_Write_Without_check(iAddress,buf,secremain);//Ğ´ÒÑ¾­²Á³ıÁËµÄ,Ö±½ÓĞ´ÈëÉÈÇøÊ£ÓàÇø¼ä.
		}
		
		if(iNumByteToWrite==secremain) //Ğ´Èë½áÊøÁË
		    break;
		else {
		    secpos += FLASH_PAGE_SIZE;
		    secoff = 0;//Æ«ÒÆÎ»ÖÃÎª0 
		    buf += secremain;  //Ö¸ÕëÆ«ÒÆ
		    iAddress += secremain;//Ğ´µØÖ·Æ«ÒÆ    
		    iNumByteToWrite -= secremain;  //×Ö½ÚÊıµİ¼õ
		    if(iNumByteToWrite>FLASH_PAGE_SIZE) secremain=FLASH_PAGE_SIZE;//ÏÂÒ»¸öÉÈÇø»¹ÊÇĞ´²»Íê
		    else secremain = iNumByteToWrite;  //ÏÂÒ»¸öÉÈÇø¿ÉÒÔĞ´ÍêÁË
		}
		
		}
        
        FLASH_Lock();
        return iNbrToWrite; 
}






/**
  * @brief  Programs a half word at a specified Option Byte Data address.
  * @note   This function can be used for all STM32F10x devices.
  * @param  Address: specifies the address to be programmed.
  * @param  buf: specifies the data to be programmed.
  * @param  iNbrToWrite: the number to read from flash
  * @retval if success return the number to write, without error
  *  
  */
int Flash_Read(uint32_t iAddress, uint8_t *buf, int32_t iNbrToRead) {
        int i = 0;
        while(i < iNbrToRead ) {
           *(buf + i) = *(uint8_t*) iAddress++;
           i++;
        }
        return i;
}
