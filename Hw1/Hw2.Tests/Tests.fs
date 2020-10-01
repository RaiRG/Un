namespace Hw2.Tests

open NUnit.Framework
open Hw2

[<TestFixture>]
type TestClass () =            
    [<Test>]   
    member this.Calculate_2Plus50_52Returned() =       
            let expected = Some(52.0)
            let actual = Hw2.Calculator.calculate "+" 50.0 2.0
            Assert.That(actual, Is.EqualTo(expected))     

    [<Test>]
    member this.Calculate_2Times8_16Returned()=        
            let expected = Some(16.0)
            let actual = Hw2.Calculator.calculate "*" 2.0 8.0
            Assert.That(actual, Is.EqualTo(expected)) 
        

    [<Test>]
    member this.Calculate_20Minus16_4Returned()=        
            let expected = Some(4.0)
            let actual = Hw2.Calculator.calculate "-" 20.0 16.0
            Assert.That(actual, Is.EqualTo(expected)) 
        

    [<Test>]
     member this.Calculate_10Divided2_5Returned()=        
            let expected = Some(5.0)
            let actual =Hw2.Calculator.calculate "/" 10.0 2.0
            Assert.That(actual, Is.EqualTo(expected)) 
     

     [<Test>]
     member this.Calculate_100Divided0_DivideByZeroNoneReturned()=        
            let expected = None
            let actual = Hw2.Calculator.calculate "/" 100.0 0.0
            Assert.That(actual, Is.EqualTo(expected)) 
         
     [<Test>]
     member this.Calculate_InvalidСharacter_NoneReturned() =        
            let expected = None
            let actual = Hw2.Calculator.calculate ">" 50.0 2.0
            Assert.That(actual, Is.EqualTo(expected))       