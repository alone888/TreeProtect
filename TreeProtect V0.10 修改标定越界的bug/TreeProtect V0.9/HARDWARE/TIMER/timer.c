
#include "timer.h"


/*******************************************************************************/	  
//将通用定时器3 配置为编码器模式

/*******************************************************************************/
void TIM3_Mode_Config(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;
	TIM_TimeBaseInitTypeDef  TIM_TimeBaseStructure;
	TIM_ICInitTypeDef TIM_ICInitStructure;
	//TIM_OCInitTypeDef  TIM_OCInitStructure;
	
	/*----------------------------------------------------------------*/
	
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOA, ENABLE);
	
	
	GPIO_StructInit(&GPIO_InitStructure);
	/* Configure PA.06,07 as encoder input */
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_6 | GPIO_Pin_7;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
	GPIO_Init(GPIOA, &GPIO_InitStructure);
	
	/*----------------------------------------------------------------*/        
	
	
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_TIM3, ENABLE); //使能ＴＩＭ３
	TIM_DeInit(TIM3);
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
	
	TIM_TimeBaseStructure.TIM_Period =0xffff;       //
	TIM_TimeBaseStructure.TIM_Prescaler =0;            //设置预分频：
	TIM_TimeBaseStructure.TIM_ClockDivision =TIM_CKD_DIV1 ;        //设置时钟分频系数：不分频
	TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;  //向上计数模式
	//TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_CenterAligned1; 
	/*初始化TIM2定时器 */
	TIM_TimeBaseInit(TIM3, &TIM_TimeBaseStructure);
	
	/*-----------------------------------------------------------------*/
	//编码配置                        编码模式
	TIM_EncoderInterfaceConfig(TIM3, TIM_EncoderMode_TI12, 
	TIM_ICPolarity_Rising, TIM_ICPolarity_Rising);  //TIM_ICPolarity_Rising上升沿捕获


	TIM_ICStructInit(&TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_1;//配置通道1的滤波器
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//比较滤波器
	TIM_ICInit(TIM3, &TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_2;//配置通道2的滤波器
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//比较滤波器
	TIM_ICInit(TIM3, &TIM_ICInitStructure);
	
	TIM_ARRPreloadConfig(TIM3, ENABLE);
	// Clear all pending interrupts
	TIM_ClearFlag(TIM3, TIM_FLAG_Update);
	TIM_ITConfig(TIM3, TIM_IT_Update, ENABLE);   //使能中断
	//Reset counter
	TIM3->CNT =0;	
	TIM_Cmd(TIM3, ENABLE);   //使能定时器3
}
void TIM_Init(void)
{
	TIM3_Mode_Config();
}
/*******************************************************************************/	  
//将高级控制定时器1 配置为编码器模式

/*******************************************************************************/
void TIM1_Mode_Config(void)
{
	GPIO_InitTypeDef GPIO_InitStructure;
	TIM_TimeBaseInitTypeDef  TIM_TimeBaseStructure;
	TIM_ICInitTypeDef TIM_ICInitStructure;
	//TIM_OCInitTypeDef  TIM_OCInitStructure;
	
	/*-------------第一步-------------------------------------------------------------------------------------------------------------------*/
	//先配置下 RCC 外设时钟 这个必须的。  这里配置对应引脚的GPIO。 timer1 对应的CH1 和 CH2 分别为 PE9 和 PE11 ，所以这里配置GPIOE
	// 一个timer 对应的有四个输入通道 CH1 ~ CH4
	/*---------------------------------------------------------------------------------------------------------------------------------------*/
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_GPIOE, ENABLE);
	
	/*-------------第二步-------------------------------------------------------------------------------------------------------------------*/
	//timer1 没有重映射的时候 CH1 和 CH2 分别对应着引脚PA8 和 PA9  怎奈何 PA9 和 PA10被用作了uart1。 所以
	//我们需要完全重映射，把CH1 和 CH2 分别对应到PE9 和 PE11。  这个做法跟以前的LPC2214不一样， 复用的引脚需要去设置这个引脚起什么作用，stm的芯片不需要
	// timer3 没有这个问题 所以不需要重映射
	/*---------------------------------------------------------------------------------------------------------------------------------------*/
	GPIO_PinRemapConfig(GPIO_FullRemap_TIM1,ENABLE);

	
	GPIO_StructInit(&GPIO_InitStructure);
	//重映射前的引脚
//	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_8 | GPIO_Pin_9;
//	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
//	GPIO_Init(GPIOA, &GPIO_InitStructure);

	//完全重映射后的引脚
	/* Configure PE.09 11 as encoder input */
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_9 | GPIO_Pin_11;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
	GPIO_Init(GPIOE, &GPIO_InitStructure);

	
	/*---------------第三步---------------------------------------------*/        
	//配置timer1， 常规设置
	//因为timer1 用的APB2（timer3用的APB1）
	RCC_APB2PeriphClockCmd(RCC_APB2Periph_TIM1,ENABLE);
	
	
	 //使能timer1
	TIM_DeInit(TIM1);
	TIM_TimeBaseInit(TIM1, &TIM_TimeBaseStructure);
	
	TIM_TimeBaseStructure.TIM_Period =0xffff;       //
	TIM_TimeBaseStructure.TIM_Prescaler =0;            				//设置预分频：
	TIM_TimeBaseStructure.TIM_ClockDivision =TIM_CKD_DIV1 ;        	//设置时钟分频系数：不分频
	TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_Up;  	//向上计数模式
	//TIM_TimeBaseStructure.TIM_CounterMode = TIM_CounterMode_CenterAligned1; 
	/*初始化TIM1定时器 */
	TIM_TimeBaseInit(TIM1, &TIM_TimeBaseStructure);
	
	/*----------------第四步-------------------------------------------------*/
	//将timer1配置为编码模式              TIM_EncoderMode_TI12的意思是AB相都用，也可以只用一相
	//TIM_EncoderInterfaceConfig(TIM1, TIM_EncoderMode_TI12, 
	//TIM_ICPolarity_Rising, TIM_ICPolarity_Rising);  //TIM_ICPolarity_Rising上升沿捕获    
	
	//---如果四细分的 应该上升下降沿都要计数 ！！！
	TIM_EncoderInterfaceConfig(TIM1, TIM_EncoderMode_TI12, 
	TIM_ICPolarity_BothEdge, TIM_ICPolarity_BothEdge);  //TIM_ICPolarity_BothEdge上升沿或者下降沿捕获  

	//  配置AB相的 数字滤波器  
	//  这个数字滤波器很重要，滤除高频干扰，就像上次的电磁干扰 。这个芯片连这个都有，给力!!
	TIM_ICStructInit(&TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_1;//配置通道1的滤波器
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//比较滤波器   
	TIM_ICInit(TIM1, &TIM_ICInitStructure);

	TIM_ICInitStructure.TIM_Channel = TIM_Channel_2;//配置通道2的滤波器
	TIM_ICInitStructure.TIM_ICFilter = 6;         	//比较滤波器
	TIM_ICInit(TIM1, &TIM_ICInitStructure);
	

	/*----------------第五步  收尾工作-------------------------------------------------*/
	TIM_ARRPreloadConfig(TIM1, ENABLE);//自动重装载配置
	// Clear all pending interrupts
	TIM_ClearFlag(TIM1, TIM_FLAG_Update);
	TIM_ITConfig(TIM1, TIM_IT_Update, ENABLE);   //使能中断
	//Reset counter
	TIM1->CNT =0;			//直接读这个counter 就是编码器的值 只有16位。 如果你想用32位的，作为程序员的你肯定能想到办法的。	
	TIM_Cmd(TIM1, ENABLE);   //使能定时器1	
}
