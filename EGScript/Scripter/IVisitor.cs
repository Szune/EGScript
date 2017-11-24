using EGScript.AbstractSyntaxTree;
using EGScript.Objects;

namespace EGScript.Scripter
{
    public interface IVisitor
    {
        void Visit(AST ast);
        void Visit(ASTClassDefinition astClass);
        void Visit(ASTMemberFunction astMemberFunction);
        void Visit(ASTGlobalFunction astFunction);
        void Visit(ASTBlock astBlock, Function function);
        void Visit(ASTContinue astContinue, Function function);
        void Visit(ASTVariableDefinition astVariableDefinition, Class _class);
        void Visit(ASTBreak astBreak, Function function);
        void Visit(ASTFunctionDefinition astFunctionDefinition, Class _class);
        void Visit(ASTIf astIf, Function function);
        void Visit(ASTReturn astReturn, Function function);
        void Visit(ASTStatementExpression astStatementExpression, Function function);
        void Visit(ASTWhile astWhile, Function function);
        void Visit(ASTExpressionBase astExpressionBase, Function function);
        void Visit(ASTCount astCount, Function function);
        void Visit(ASTGlobalFunctionCall astGlobalFunctionCall, Function function);
        void Visit(ASTMemberFunctionCall astMemberFunctionCall, Function function);
        void Visit(ASTNew astNew, Function function);
        void Visit(ASTNumber astNumber, Function function);
        void Visit(ASTNull astNull, Function function);
        void Visit(ASTTrue astTrue, Function function);
        void Visit(ASTFalse astFalse, Function function);
        void Visit(ASTTable astTable, Function function);
        void Visit(ASTTableElement astTableElement, Function function);
        void Visit(ASTTableGet astTableGet, Function function);
        void Visit(ASTString astString, Function function);
        void Visit(ASTIdentifier astIdentifier, Function function);
        void Visit(ASTAssignment astAssignment, Function function);
        void Visit(ASTBinaryMathOperation astBinaryMathOperation, Function function);
        void Visit(ASTUnaryMathOperation astUnaryMathOperation, Function function);
        void Visit(ASTCompare astCompare, Function function);
        void Visit(ASTFor astFor, Function function);
        void Visit(ASTSwitch astSwitch, Function function);
        void Visit(ASTGlobalVariableAssignment astGlobalVariableAssignment);
    }
}