using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fody;
using Mono.Cecil;

public partial class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        Initialize();

        var typesToWeave =
            ModuleDefinition
                .GetTypes()
                .SelectMany(MethodsToWeave)
                .GroupBy(method => method.DeclaringType);

        foreach (var grouping in typesToWeave)
        {
            var declaringType = grouping.Key;

            foreach (var weaverInfo in grouping.Select(WeaverInformation))
            {
                switch (weaverInfo.WeaverKind)
                {
                    case WeaverKind.Void:
                        WeaveInvokeOnMainThreadOnVoidMethod(weaverInfo.MethodDefinition, declaringType);
                        break;

                    case WeaverKind.None:
                        WriteError(ReturnTypeMustBeVoid, weaverInfo.SequencePoint);
                        break;
                }

                //Removes the reference to the RunOnMainThread attribute
                weaverInfo.MethodDefinition.CustomAttributes.Remove(weaverInfo.MethodDefinition.CustomAttributes
                    .First(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName));
            }
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "RunOnMainThread";
        yield return "MainThreadDispatcher";
        yield return "RunOnMainThread"; //Todo: Not sure if this needs to be here
        yield return "netstandard";
        yield return "mscorlib";
    }

    private void Initialize()
    {
        //Types
        Void = TypeSystem.VoidReference;
        Bool = TypeSystem.VoidReference;

        MainThreadDispatcher = FindTypeDefinition(MainThreadDispatcherTypeName);
        Action = FindTypeDefinition(ActionTypeName);
        MainThreadAttribute = FindTypeDefinition(RunOnMainThreadAttributeTypeName);

        //Methods
        ActionConstructor = ModuleDefinition.ImportReference(typeof(Action).GetConstructors().Single());

        RunOnMainThread = new MethodReference(RunOnMainThreadName, Void, MainThreadDispatcher);
        RunOnMainThread.Parameters.Add(new ParameterDefinition(Action));
    }


    private IEnumerable<MethodDefinition> MethodsToWeave(TypeDefinition type)
        => type.Methods.Where(MethodHasMainThreadAttribute);

    private bool MethodHasMainThreadAttribute(MethodDefinition method)
        => method.CustomAttributes.Any(a => a.AttributeType.Name == RunOnMainThreadAttributeTypeName);
}