using System.Linq;
using Mono.Cecil;
using static WeaverInfo;

public partial class ModuleWeaver
{
    private WeaverInfo WeaverInformation(MethodDefinition method)
    {
        //// Can't use this old code because Void is a Reference Type and will not ever be the same as method.ReturnType:
        //if (method.ReturnType == Void)
        //    return Void(method);

        if (method.ReturnType.FullName == Void.FullName) //Todo: Fix this. This is a really hackey fix that probably has all kinds of edge cases.
            return Void(method);

        var sequencePoint = method.DebugInformation.SequencePoints.FirstOrDefault();
        return None(sequencePoint);
    }
}
