// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace Pvpdata
{

using global::System;
using global::FlatBuffers;

public struct Player : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Player GetRootAsPlayer(ByteBuffer _bb) { return GetRootAsPlayer(_bb, new Player()); }
  public static Player GetRootAsPlayer(ByteBuffer _bb, Player obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Player __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public PlayerStart Startstate { get { int o = __p.__offset(4); return o != 0 ? (PlayerStart)__p.bb.GetSbyte(o + __p.bb_pos) : PlayerStart.Match; } }
  public int Userindex { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string Id { get { int o = __p.__offset(8); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetIdBytes() { return __p.__vector_as_span(8); }
#else
  public ArraySegment<byte>? GetIdBytes() { return __p.__vector_as_arraysegment(8); }
#endif
  public byte[] GetIdArray() { return __p.__vector_as_array<byte>(8); }
  public string Nickname { get { int o = __p.__offset(10); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetNicknameBytes() { return __p.__vector_as_span(10); }
#else
  public ArraySegment<byte>? GetNicknameBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public byte[] GetNicknameArray() { return __p.__vector_as_array<byte>(10); }
  public int Level { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Power { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Hp { get { int o = __p.__offset(16); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Roomnumber { get { int o = __p.__offset(18); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Roomindex { get { int o = __p.__offset(20); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public Vec3? Playerpos { get { int o = __p.__offset(22); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public float Playerrot { get { int o = __p.__offset(24); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public MoveState Movestate { get { int o = __p.__offset(26); return o != 0 ? (MoveState)__p.bb.GetSbyte(o + __p.bb_pos) : MoveState.Move; } }
  public Vec3? Destinationpos { get { int o = __p.__offset(28); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public int Attacked { get { int o = __p.__offset(30); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Fire { get { int o = __p.__offset(32); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Currenthp { get { int o = __p.__offset(34); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Gunid { get { int o = __p.__offset(36); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Armorid { get { int o = __p.__offset(38); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Skill1id { get { int o = __p.__offset(40); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Skill2id { get { int o = __p.__offset(42); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int Skillfire { get { int o = __p.__offset(44); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static void StartPlayer(FlatBufferBuilder builder) { builder.StartObject(21); }
  public static void AddStartstate(FlatBufferBuilder builder, PlayerStart startstate) { builder.AddSbyte(0, (sbyte)startstate, 0); }
  public static void AddUserindex(FlatBufferBuilder builder, int userindex) { builder.AddInt(1, userindex, 0); }
  public static void AddId(FlatBufferBuilder builder, StringOffset idOffset) { builder.AddOffset(2, idOffset.Value, 0); }
  public static void AddNickname(FlatBufferBuilder builder, StringOffset nicknameOffset) { builder.AddOffset(3, nicknameOffset.Value, 0); }
  public static void AddLevel(FlatBufferBuilder builder, int level) { builder.AddInt(4, level, 0); }
  public static void AddPower(FlatBufferBuilder builder, int power) { builder.AddInt(5, power, 0); }
  public static void AddHp(FlatBufferBuilder builder, int hp) { builder.AddInt(6, hp, 0); }
  public static void AddRoomnumber(FlatBufferBuilder builder, int roomnumber) { builder.AddInt(7, roomnumber, 0); }
  public static void AddRoomindex(FlatBufferBuilder builder, int roomindex) { builder.AddInt(8, roomindex, 0); }
  public static void AddPlayerpos(FlatBufferBuilder builder, Offset<Vec3> playerposOffset) { builder.AddStruct(9, playerposOffset.Value, 0); }
  public static void AddPlayerrot(FlatBufferBuilder builder, float playerrot) { builder.AddFloat(10, playerrot, 0.0f); }
  public static void AddMovestate(FlatBufferBuilder builder, MoveState movestate) { builder.AddSbyte(11, (sbyte)movestate, 0); }
  public static void AddDestinationpos(FlatBufferBuilder builder, Offset<Vec3> destinationposOffset) { builder.AddStruct(12, destinationposOffset.Value, 0); }
  public static void AddAttacked(FlatBufferBuilder builder, int attacked) { builder.AddInt(13, attacked, 0); }
  public static void AddFire(FlatBufferBuilder builder, int fire) { builder.AddInt(14, fire, 0); }
  public static void AddCurrenthp(FlatBufferBuilder builder, int currenthp) { builder.AddInt(15, currenthp, 0); }
  public static void AddGunid(FlatBufferBuilder builder, int gunid) { builder.AddInt(16, gunid, 0); }
  public static void AddArmorid(FlatBufferBuilder builder, int armorid) { builder.AddInt(17, armorid, 0); }
  public static void AddSkill1id(FlatBufferBuilder builder, int skill1id) { builder.AddInt(18, skill1id, 0); }
  public static void AddSkill2id(FlatBufferBuilder builder, int skill2id) { builder.AddInt(19, skill2id, 0); }
  public static void AddSkillfire(FlatBufferBuilder builder, int skillfire) { builder.AddInt(20, skillfire, 0); }
  public static Offset<Player> EndPlayer(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Player>(o);
  }
};


}
