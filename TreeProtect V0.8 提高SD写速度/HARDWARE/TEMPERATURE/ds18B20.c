#include "ds18b20.h"
#include "delay.h"	


/*
 * 函数名：DS18B20_GPIO_Config
 * 描述  ：配置DS18B20用到的I/O口
 * 输入  ：无
 * 输出  ：无
 */
 void DS18B20_GPIO_Config(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;	//GPIO
	RCC_APB2PeriphClockCmd(	RCC_APB2Periph_GPIOG, ENABLE );	 //使能PORTA口时钟  
 
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;  //SPI CS
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;  //复用推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_Init(GPIOG, &GPIO_InitStructure);
	/* Deselect the PA0 Select high */
	GPIO_SetBits(GPIOG,GPIO_Pin_0);
}
/*
 * 函数名：DS18B20_Mode_IPU
 * 描述  ：使DS18B20-DATA引脚变为输入模式
 * 输入  ：无
 * 输出  ：无
 */
void DS18B20_Mode_IPU(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;	//GPIO
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;  //SPI CS
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IPU;  //复用推挽输出
	GPIO_Init(GPIOG, &GPIO_InitStructure);

}
/*
 * 函数名：DS18B20_Mode_Out
 * 描述  ：使DS18B20-DATA引脚变为输出模式
 * 输入  ：无
 * 输出  ：无
 */
void DS18B20_Mode_Out(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;	//GPIO
  GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9;  //SPI CS
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;  //复用推挽输出
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_Init(GPIOG, &GPIO_InitStructure);

}



/*
 *主机给从机发送复位脉冲
 */
void DS18B20_Rst(void)	   
{        
    /* IO设置为推挽输出*/	
	  DS18B20_Mode_Out(); 
	  /*产生至少480us的低电平复位信号 */
    DS18B20_DQ_OUT_Low; 
		delay_us(480);  
    /* 在产生复位信号后，需将总线拉高 */	
    DS18B20_DQ_OUT_High; 
	  delay_us(15);    
}

/*
 * 检测从机给主机返回的应答脉冲
 *从机接收到主机的复位信号后，会在15~60us后给主机发一个应答脉冲
 * 0：成功
 * 1：失败
 */
u8 DS18B20_Answer_Check(void) 	   
{   
	u8 delay=0;
	/* 主机设置为上拉输入 */
	DS18B20_Mode_IPU(); 
	/* 等待应答脉冲（一个60~240us的低电平信号 ）的到来
	 * 如果100us内，没有应答脉冲，退出函数，注意：从机接收到主机的复位信号后，会在15~60us后给主机发一个存在脉冲
	 */
	while (DS18B20_DQ_IN&&delay<100)
	{
		delay++;
		delay_us(1);
	}	 
	/*经过100us后，如果没有应答脉冲，退出函数*/	
	if(delay>=100)
		return 1;
	else 
		delay=0;
	/*有应答脉冲，且存在时间不超过240us */
	while (!DS18B20_DQ_IN&&delay<240)
	{
		delay++;
		delay_us(1);
	}
	if(delay>=240)
		return 1;	    
	  return 0;
}

//从DS18B20读取一个位
//返回值：1/0
u8 DS18B20_Read_Bit(void) 			 // read one bit
{
  u8 data;
	DS18B20_Mode_Out();
	/* 读时间的起始：必须由主机产生 >1us <15us 的低电平信号 */
  DS18B20_DQ_OUT_Low; 
	delay_us(2);
	DS18B20_DQ_OUT_High; 
		delay_us(12);
	/* 设置成输入，释放总线，由外部上拉电阻将总线拉高 */
	DS18B20_Mode_IPU();

	if(DS18B20_DQ_IN)
		data=1;
  else 
		data=0;	 
  delay_us(50);           
  return data;
}
//从DS18B20读取一个字节
//返回值：读到的数据
u8 DS18B20_Read_Byte(void)    // read one byte
{        
    u8 i,j,dat;
    dat=0;
	for(i=0; i<8; i++) 
	{
		j = DS18B20_Read_Bit();		
		dat = (dat) | (j<<i);
	}					    
    return dat;
}
/*
 * 写一个字节到DS18B20
 */
void DS18B20_Write_Byte(u8 dat)     
 {             
    u8 j;
    u8 testb;
	DS18B20_Mode_Out();//SET PA0 OUTPUT;
    for (j=1;j<=8;j++) 
	{
        testb=dat&0x01;
        dat=dat>>1;
        if (testb) 
        {
            DS18B20_DQ_OUT_Low;// Write 1
            delay_us(10);                            
            DS18B20_DQ_OUT_High;
            delay_us(50);             
        }
        else 
        {
            DS18B20_DQ_OUT_Low;// Write 0
            delay_us(60);             
            DS18B20_DQ_OUT_High;   ///释放总线
            delay_us(2);                          
        }
    }
}

//初始化DS18B20的IO口 DQ 同时检测DS的存在
//返回1:不存在
//返回0:存在    	 
u8 DS18B20_Init(void)
{
  DS18B20_GPIO_Config();
	DS18B20_Rst();
	return DS18B20_Answer_Check();
}  
//从ds18b20得到温度值
//精度：0.1C
//返回值：温度值 （-550~1250） 
short DS18B20_Get_Temp(void)
{
    
	u8 flag;
	u8 TL,TH;
	short Temperature;
	float Temperature1;
	DS18B20_Rst();	   
	DS18B20_Answer_Check();	 
	DS18B20_Write_Byte(0xcc);// skip rom
	DS18B20_Write_Byte(0x44);// convert                 // ds1820 start convert
	DS18B20_Rst();
	DS18B20_Answer_Check();	 
	DS18B20_Write_Byte(0xcc);// skip rom
	DS18B20_Write_Byte(0xbe);// convert	    
	TL=DS18B20_Read_Byte(); // LSB   
	TH=DS18B20_Read_Byte(); // MSB  
	if( TH&0xfc)
	{
	flag=1;
	Temperature=(TH<<8)|TL;
	Temperature1=(~ Temperature)+1;
	Temperature1*=0.0625;
	}
	else
	{
	flag=0;
	Temperature1=((TH<<8)|TL)*0.0625;
	}
	Temperature = Temperature1*100;
	return Temperature; 	
} 
 
