namespace Algs
{
	class Program
	{
		static void Main(string[] args)
		{
			SortedSubrangeFinderTests.BasicTest();
			SortedSubrangeFinderTests.LongTest(100, 3, 1000);
			RMQPM1Test.BasicTest();
			RMQPM1Test.LongTest(100, 130, 1000);
			RMQTest.BasicTest();
			RMQTest.LongTest(300, 1300, 1000);
			RMQTest.LongGetBoundedTest(30, 130, 1000);
		}
	}
}
