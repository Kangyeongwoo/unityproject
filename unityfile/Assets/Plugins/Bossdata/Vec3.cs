// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Bossdata
{

using global::System;
using global::FlatBuffers;

public struct Vec3 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Vec3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float X { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float Y { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float Z { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }

  public static Offset<Vec3> CreateVec3(FlatBufferBuilder builder, float X, float Y, float Z) {
    builder.Prep(4, 12);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    return new Offset<Vec3>(builder.Offset);
  }
};


}
