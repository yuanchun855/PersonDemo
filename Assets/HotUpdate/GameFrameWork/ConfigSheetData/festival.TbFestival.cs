
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.festival
{
public partial class TbFestival
{
    private readonly System.Collections.Generic.Dictionary<int, Festival> _dataMap;
    private readonly System.Collections.Generic.List<Festival> _dataList;
    
    public TbFestival(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, Festival>();
        _dataList = new System.Collections.Generic.List<Festival>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            Festival _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = Festival.DeserializeFestival(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, Festival> DataMap => _dataMap;
    public System.Collections.Generic.List<Festival> DataList => _dataList;

    public Festival GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Festival Get(int key) => _dataMap[key];
    public Festival this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}