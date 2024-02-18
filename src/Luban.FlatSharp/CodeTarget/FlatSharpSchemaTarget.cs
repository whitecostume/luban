using Luban;
using Luban.CodeFormat;
using Luban.CodeTarget;
using Luban.Defs;
using Luban.FlatBuffers.TemplateExtensions;
using Luban.FlatBuffers.TypeVisitors;
using Luban.Tmpl;
using Scriban;
using Scriban.Runtime;


[CodeTarget("flatsharp")]
public class FlatSharpSchemaTarget : AllInOneTemplateCodeTargetBase
{
    public override string FileHeader => "";

    protected override string FileSuffixName => "fbs";

    protected override ICodeStyle CodeStyle => CodeFormatManager.Ins.NoneCodeStyle;

    protected override string DefaultOutputFileName => "schema.fbs";

    protected override void OnCreateTemplateContext(TemplateContext ctx)
    {
        ctx.PushGlobal(new FlatSharpTemplateExtension());

        var maps = CollectKeyValueEntry(GenerationContext.Current.ExportBeans).KeyValueEntries.Values;
        ctx.PushGlobal(new ScriptObject()
        {
            {"__maps", maps},
        });
    }

    private MapKeyValueEntryCollection CollectKeyValueEntry(List<DefBean> beans)
    {
        var c = new MapKeyValueEntryCollection();

        foreach (DefBean bean in beans)
        {
            CollectMapKeyValueEntriesVisitor.Ins.Accept(bean, c);
        }

        return c;
    }
}
