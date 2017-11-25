using EGScript;
using EGScript.Helpers;
using EGScript.Objects;
using EGScript.Scripter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace EGScriptTest
{
    [TestClass]
    public class InterpreterTest
    {
        TestPrinter printer => Settings.Printer as TestPrinter;

        [TestInitialize]
        public void InitializeTest()
        {
            Settings.Reset();
            Settings.Printer = new TestPrinter();
        }

        [TestMethod]
        public void Main_Function_Returns_String()
        {
            var script = new Script(@"
function test()
{
    var2 = 10;
    print(var2);
    return 5;
}
function main()
{
    var4 = test();
    var1 = 10;
    var2 = "" jez"";
    print(var1 + "" hej"");
    print(""hej"" + var2);
    print("" - - "");
return var1 + var2;
}");
            script.Run().As<StringObj>().Text.Should().Be("10 jez");
        }

        [TestMethod]
        public void Main_Function_Returns_Number()
        {
            var script = new Script(@"
function main()
{
    var1 = 10;
    var2 = "" jez"";
    print(var1 + "" hej"");
    print(""hej"" + var2);
    print("" - - "");
    return 5;
}").Run();

            script.As<Number>().Value.Should().Be(5);
        }

        [TestMethod]
        public void Function_Returns_Value_And_Sets_Value_To_Variable()
        {
            var script = new Script(@"

function main()
{
    var = test();
    return var;
}

function test2()
{
    return 10;
}

function test()
{
    return test2();
}");
            script.Run().As<Number>().Value.Should().Be(10);
        }

        [TestMethod]
        public void Main_Function_Returns_Nothing()
        {
            var script = new Script(@"function main()
{
    print(""oh hi script"");
}");
            script.Run().Should().BeOfType<Null>();
        }

        [TestMethod]
        public void Main_Function_Returns_True()
        {
            var script = new Script(@"function main()
{
    return true;
}");
            script.Run().Should().BeOfType<True>();
        }

        [TestMethod]
        public void Main_Function_Returns_False()
        {
            var script = new Script(@"function main()
{
    return false;
}");
            script.Run().Should().BeOfType<False>();
        }

        [TestMethod]
        public void Addition_Assignment_Operator_Returns_Value_Added_By_Added_Amount()
        {
            var script = new Script(@"function main()
{
    var = 5;
    var += 6;
    return var;
}");
            var run = script.Run();
            run.As<Number>().Value.Should().Be(11);
        }


        [TestMethod]
        public void Addition_Assignment_Operator_Two_In_A_Row_Should_Throw_ParserException()
        {
            Action script = () => new Script(@"function main()
{
    var = 5;
    var += 6 += 6;
    return var;
}");
            script.Should().Throw<ParserException>();
        }

        [TestMethod]
        public void Subtraction_Assignment_Operator_Two_In_A_Row_Should_Throw_ParserException()
        {
            Action script = () => new Script(@"function main()
{
    var = 5;
    var -= 6 -= 6;
    return var;
}");
            script.Should().Throw<ParserException>();
        }

        [TestMethod]
        public void Subtraction_Assignment_Operator_Returns_Value_Subtracted_By_Subtracted_Amount()
        {
            var script = new Script(@"function main()
{
    var = 6;
    var -= 3;
    return var;
}");
            var run = script.Run();
            run.As<Number>().Value.Should().Be(3);
        }

        [TestMethod]
        public void Function_With_Parameters_Returns_String()
        {
            var script = new Script(@"function main()
{
    return test(""Erik"");
}

function test(namn)
{
    return ""Good morning, "" + namn + ""."";
}
");
            var run = script.Run();
            run.As<StringObj>().Text.Should().Be("Good morning, Erik.");
        }

        [TestMethod]
        public void Function_With_2_Parameters_Returns_String()
        {
            var script = new Script(@"function main()
{
    return test(""Erik"", 24);
}

function test(namn, age)
{
    return ""Good morning, "" + namn + "". Wow this is a very useful function because it outputs your own age!! (and it is "" + age + "")"";
}
");
            var run = script.Run();
            run.As<StringObj>().Text.Should().Be("Good morning, Erik. Wow this is a very useful function because it outputs your own age!! (and it is 24)");
        }

        [TestMethod]
        public void Random_Should_Throw_InterpreterException_If_No_Parameters()
        {
            var script = new Script(@"function main()
{
    return random();
}");
            Action run = () => script.Run();
            run.Should().Throw<InterpreterException>();
        }


        [TestMethod]
        public void Random_Should_Throw_InterpreterException_If_Too_Many_Parameters()
        {
            var script = new Script(@"function main()
{
    return random(1,2,3);
}");
            Action run = () => script.Run();
            run.Should().Throw<InterpreterException>();
        }

        [TestMethod]
        public void Random_Should_Not_Throw_InterpreterException_With_1_Parameter()
        {
            var script = new Script(@"function main()
{
    return random(1);
}");
            Action run = () => script.Run();
            run.Should().NotThrow<InterpreterException>();
        }

        [TestMethod]
        public void Random_Should_Not_Throw_InterpreterException_With_2_Parameters()
        {
            var script = new Script(@"function main()
{
    return random(1,2);
}");
            Action run = () => script.Run();
            run.Should().NotThrow<InterpreterException>();
        }

        [TestMethod]
        public void Random_Should_Return_Value_In_Range_0_To_5_If_Called_With_1_Parameter_With_Value_Of_5()
        {
            var script = new Script(@"function main()
{
    return random(5);
}");
            var run = script.Run();
            run.As<Number>().Value.Should().BeInRange(0, 5);
        }

        [TestMethod]
        public void Random_Should_Return_Value_In_Range_3_To_7_If_Called_With_2_Parameters_With_Value_Of_3_And_7()
        {
            var script = new Script(@"function main()
{
    return random(3,7);
}");
            var run = script.Run();
            run.As<Number>().Value.Should().BeInRange(3, 7);
        }

        [TestMethod]
        public void Random_Throws_ScriptException_If_Variable_Is_Not_Number()
        {
            var script = new Script(@"function main()
{
    return random(""s"");
}");
            Action run = () => script.Run();
            run.Should().Throw<ScriptException>();
        }

        [TestMethod]
        public void True_Equals_True()
        {
            var script = new Script(@"function main()
{
    return true == true;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void True_Does_Not_Equal_False()
        {
            var script = new Script(@"function main()
{
    return true == false;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void True_Does_Not_Equal_Null()
        {
            var script = new Script(@"function main()
{
    return true == null;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void Null_Equals_Null()
        {
            var script = new Script(@"function main()
{
    return null == null;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void False_Equals_False()
        {
            var script = new Script(@"function main()
{
    return false == false;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void Null_Equals_False()
        {
            var script = new Script(@"function main()
{
    return null == false;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void Two_Equals_Two()
        {
            var script = new Script(@"function main()
{
    return 2 == 2;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void Two_Does_Not_Equal_One()
        {
            var script = new Script(@"function main()
{
    return 2 == 1;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void Hello_Equals_Hello()
        {
            var script = new Script(@"function main()
{
    return ""Hello"" == ""Hello"";
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void Hello_Does_Not_Equal_GoodBye()
        {
            var script = new Script(@"function main()
{
    return ""Hello"" == ""GoodBye"";
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void StringComparison_Is_Case_Sensitive()
        {
            var script = new Script(@"function main()
{
    return ""Hello"" == ""hello"";
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void StringComparison_Is_Case_Sensitive_On_Both_Sides()
        {
            var script = new Script(@"function main()
{
    return ""hello"" == ""Hello"";
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void StringComparison_Strings_Are_Not_Equals_So_Not_Equals_Should_Return_True()
        {
            var script = new Script(@"function main()
{
    return ""hello"" != ""Hello"";
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void Number_And_String_That_Does_Not_Hold_The_Same_Number_Should_Return_False()
        {
            var script = new Script(@"function main()
{
    return ""Hello"" == 6464;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void Number_Two_Is_LessThanEquals_Five()
        {
            var script = new Script(@"function main()
{
    return 2 <= 5;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void Number_Six_Is_Not_LessThanEquals_Five()
        {
            var script = new Script(@"function main()
{
    return 6 <= 5;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void Number_Three_Is_LessThan_Seven()
        {
            var script = new Script(@"function main()
{
    return 3 < 7;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void Number_Eight_Is_Not_LessThan_Seven()
        {
            var script = new Script(@"function main()
{
    return 8 < 7;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void String_Two_Is_LessThanEquals_Five()
        {
            var script = new Script(@"function main()
{
    return ""2"" <= 5;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void String_Six_Is_Not_LessThanEquals_Five()
        {
            var script = new Script(@"function main()
{
    return ""6"" <= 5;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void String_Three_Is_LessThan_Seven()
        {
            var script = new Script(@"function main()
{
    return ""3"" < 7;
}");
            var run = script.Run();
            run.Should().BeOfType<True>();
        }

        [TestMethod]
        public void String_Eight_Is_Not_LessThan_Seven()
        {
            var script = new Script(@"function main()
{
    return ""8"" < 7;
}");
            var run = script.Run();
            run.Should().BeOfType<False>();
        }

        [TestMethod]
        public void Multiplication_Of_7_And_3_Should_Return_21()
        {
            var script = new Script(@"function main()
{
    return 7 * 3;
}");
            var run = script.Run();
            run.As<Number>().Value.Should().Be(21);
        }

        [TestMethod]
        public void Include_Should_Add_Included_Classes()
        {
            // class implementation hasn't been verified yet
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Include_Should_Add_Included_Functions()
        {
            var includedScript = @"function includedFunction(arg)
{
    return arg + 5;
}";
            var fileToInclude = new Mock<IFileHandler>();
            fileToInclude.Setup(c => c.ReadFileToEnd(It.Is<string>(s => s == "includeFile.soup"))).Returns(includedScript);
            fileToInclude.Setup(c => c.WorkingDirectory).Returns("");
            var script = new Script(@"include ""includeFile.soup"";

function main()
{
    return includedFunction(10);
}", fileToInclude.Object);
            var run = script.Run();
            run.As<Number>().Value.Should().Be(15);
        }


        [TestMethod]
        public void IntegrationTest_Include_Should_Work_With_Relative_Paths_Even_For_Nested_Includes()
        {
            /* Integration test */
            var script = new Script(@"include ""scripts\script.txt"";

function main()
{
    return inc(10);
}");
            var run = script.Run();
            run.As<Number>().Value.Should().Be(22);
        }

        [TestMethod]
        public void IntegrationTest_Include_Should_Work_With_Absolute_Paths()
        {
            /* Integration test */
            var script = new Script(@"include ""scripts\script3.txt"";

function main()
{
    return twice(10);
}");
            var run = script.Run();
            run.As<Number>().Value.Should().Be(5);
        }

        [TestMethod]
        public void Switch_Statement_Should_Fall_Through_If_No_Break_Specified()
        {
            var script = new Script(@"function main()
{
    //var = {""hej"", 5};
    var = 10;
    switch(var)
    {
        case 10:
            print(var);
        case 5:
            print(var);
    }
}");
            // fallthrough /\
            var run = script.Run();
            printer.PrintedMessages.Should().BeEquivalentTo(new[] { "10", "10" });
        }

        [TestMethod]
        public void Switch_Statement_Should_Not_Fall_Through_If_Break_Specified()
        {
            var script = new Script(@"function main()
{
    var = 15;
    switch(var)
    {
        case 15:
            print(var);
            break;
        case 5:
            print(var);
            break;
    }
}");
            // fallthrough /\
            var run = script.Run();
            printer.PrintedMessages.Should().BeEquivalentTo(new[] { "15" });
        }

        [TestMethod]
        public void Switch_Statement_Should_Work_With_Braces_In_Cases()
        {
            var script = new Script(@"function main()
{
    var = 17;
    switch(var)
    {
        case 17:
            {
                print(var);
                break;
            }
        case 5:
            {
                print(""hej"");
                break;
            }
    }
}");
            // fallthrough /\
            var run = script.Run();
            printer.PrintedMessages.Should().BeEquivalentTo(new[] { "17" });
        }

        [TestMethod]
        public void Multiplication_Assignment_Operator_Returns_Value_Multiplied_By_Amount()
        {
            var script = new Script(@"function main()
{
    var = 5;
    var *= 5;
    return var;
}");
            var run = script.Run();
            run.As<Number>().Value.Should().Be(25);
        }

        [TestMethod]
        public void Division_Assignment_Operator_Returns_Value_Divided_By_Amount()
        {
            var script = new Script(@"function main()
{
    var = 20;
    var /= 5;
    return var;
}");
            var run = script.Run();
            run.As<Number>().Value.Should().Be(4);
        }


        [TestMethod]
        public void Table_Assignment_Only_Value_Should_Be_OKIDOKI()
        {
            //            var script = new Script(@"function main()
            //{
            //    var = {""hej"", 5, 7};
            //    newTable = var[0,2];
            //    var count = newTable->Count();
            //    print(newTable->Count());
            //}");
            var script = new Script(@"function main()
{
    tbb = {[""hej""] = ""säg hej""};
    print(tbb[""hej""]);
    newTable = {1,2,3,4,5,10};
    nTable = newTable[0,4,2];
    print(#nTable);
    print(newTable[4]);
    count = #newTable;
    print(count);

    for(i = 0; i < #nTable; i++)
    {
        print(""nTable["" + i + ""]: "" + nTable[i]);
    }
}");

            var run = script.Run();
            Assert.Fail("add more tests");
        }

        [TestMethod]
        public void GlobalVar_Test()
        {
            var script = new Script(@"
global
{
    var1 = 3246;
    var2 = ""hellooo"";
    var3 = wha();
}

function wha()
{
    return 5 * 10;
}

function main()
{
    print(var1);
    print(var2);
    print(var3);
}");
            var run = script.Run();
            printer.PrintedMessages[0].Should().Be("3246");
            printer.PrintedMessages[1].Should().Be("hellooo");
            printer.PrintedMessages[2].Should().Be("50");
        }

        [TestMethod]
        public void IntegrationTest_GlobalVar_In_Include()
        {
            var includedScript = @"global
{
    ITERATION_COUNT = 10;
    ITERATION_NAME = ""TEST"";
}
";
            var incScript2 = @"
include ""includeFile.soup"";

function doWork()
{
    for(i = 0; i < ITERATION_COUNT; i++)
    {
        print(""doWork: "" + i);
    }
}
";
            var fileToInclude = new Mock<IFileHandler>();
            fileToInclude.Setup(c => c.ReadFileToEnd(It.Is<string>(s => s == "includeFile.soup"))).Returns(includedScript);
            fileToInclude.Setup(c => c.ReadFileToEnd(It.Is<string>(s => s == "includeFile2.soup"))).Returns(incScript2);
            fileToInclude.Setup(c => c.WorkingDirectory).Returns("");
            fileToInclude.Setup(c => c.Copy(It.IsAny<string>())).Returns(fileToInclude.Object);
            var script = new Script(@"
include ""includeFile.soup"";
include ""includeFile2.soup"";

global
{
    ITERATION_COUNT = 10;
}

function main()
{
    for(i = 0; i < ITERATION_COUNT; i++)
    {
        print(""main:"" + i);
    }
    doWork();
}", fileToInclude.Object);

            var run = script.Run();
            printer.PrintedMessages.Should().HaveCount(20);
        }

        [TestMethod]
        public void IntegrationTest_GlobalVar_In_Include_Different_Values()
        {
            var includedScript = @"global
{
    ITERATION_COUNT = 10;
    ITERATION_NAME = ""TEST"";
}
";
            var incScript2 = @"
include ""includeFile.soup"";

function doWork()
{
    for(i = 0; i < ITERATION_COUNT; i++)
    {
        print(""doWork: "" + i);
    }
}
";
            var fileToInclude = new Mock<IFileHandler>();
            fileToInclude.Setup(c => c.ReadFileToEnd(It.Is<string>(s => s == "includeFile.soup"))).Returns(includedScript);
            fileToInclude.Setup(c => c.ReadFileToEnd(It.Is<string>(s => s == "includeFile2.soup"))).Returns(incScript2);
            fileToInclude.Setup(c => c.WorkingDirectory).Returns("");
            fileToInclude.Setup(c => c.Copy(It.IsAny<string>())).Returns(fileToInclude.Object);
            var script = new Script(@"
include ""includeFile.soup"";
include ""includeFile2.soup"";

global
{
    ITERATION_COUNT = 5;
}

function main()
{
    for(i = 0; i < ITERATION_COUNT; i++)
    {
        print(""main:"" + i);
    }
    doWork();
}", fileToInclude.Object);

            Action run = () => script.Run();
            run.Should().ThrowExactly<InterpreterException>();
        }
    }
}
