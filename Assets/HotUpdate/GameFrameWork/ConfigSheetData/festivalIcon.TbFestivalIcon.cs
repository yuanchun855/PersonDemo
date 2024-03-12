
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.festivalIcon
{
public partial class TbFestivalIcon
{
    private readonly System.Collections.Generic.Dictionary<int, FestivalIcon> _dataMap;
    private readonly System.Collections.Generic.List<FestivalIcon> _dataList;
    
    public TbFestivalIcon(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, FestivalIcon>();
        _dataList = new System.Collections.Generic.List<FestivalIcon>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            FestivalIcon _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = FestivalIcon.DeserializeFestivalIcon(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, FestivalIcon> DataMap => _dataMap;
    public System.Collections.Generic.List<FestivalIcon> DataList => _dataList;

    public FestivalIcon GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public FestivalIcon Get(int key) => _dataMap[key];
    public FestivalIcon this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}
