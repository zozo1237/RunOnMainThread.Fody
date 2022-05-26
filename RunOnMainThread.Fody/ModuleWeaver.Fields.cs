using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

public partial class ModuleWeaver
{
    private const string ReturnTypeMustBeVoid = "You can only use the RunOnMainThreadAttribute on void methods";

    private const string ActionTypeName = "Action";
    private const string SystemAssemblyName = "System";
    private const string ConstructorMethodName = ".ctor";
    private const string RunOnMainThreadName = "RunOnMainThread";
    private const string RunOnMainThreadAssemblyName = "RunOnMainThread";
    private const string RunOnMainthreadNamespaceName = "RunOnMainThread";
    private const string MainThreadDispatcherTypeName = "MainThreadDispatcher";
    private const string RunOnMainThreadAttributeTypeName = "RunOnMainThreadAttribute";

    private static AssemblyNameReference RunOnMainThreadAssembly;

    private TypeReference Bool;
    private TypeReference Void;
    private TypeDefinition Action;
    private TypeDefinition MainThreadAttribute;
    private TypeDefinition MainThreadDispatcher;

    private MethodReference RunOnMainThread;
    private MethodReference ActionConstructor;
}