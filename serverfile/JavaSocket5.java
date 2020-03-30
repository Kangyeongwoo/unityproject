import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.util.Collection;
import java.util.HashMap;
import java.util.Iterator;
import com.google.flatbuffers.FlatBufferBuilder;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.ArrayList;
import java.util.Iterator;
import java.nio.ByteBuffer;
import Pvpdata.Game;
import Pvpdata.Item;
import Pvpdata.Mapeffect;
import Pvpdata.Player;
import Pvpdata.PlayerStart;
import Pvpdata.MoveState;
import Pvpdata.Vec3;
import java.net.SocketTimeoutException;
import java.lang.NullPointerException;
public class JavaSocket5 {

    static int count = 1;

    public static void main(String[] args){
     try{
       //소켓 생성 4343포트 입력 10개까지 (queue)

       ServerSocket  serverSocket = new ServerSocket(4343, 10);
       System.out.println("Socket time: "+serverSocket.getSoTimeout());
       HashMap<OutputStream,OutputStream> hm= new HashMap<OutputStream,OutputStream>();
       HashMap<Integer,List<OutputStream>> Room = new HashMap<Integer,List<OutputStream>>();
       HashMap<OutputStream,Integer> Roomnumber = new HashMap<OutputStream,Integer>();
       HashMap<OutputStream,Integer> userindex = new HashMap<OutputStream,Integer>();
       HashMap<OutputStream,byte[]> userdata = new HashMap<OutputStream,byte[]>();

       while(true) {
                       //
                       Socket socket = serverSocket.accept();

                       try{
                       System.out.println("1, wait ");
                       System.out.println("2, connet :"+socket.getRemoteSocketAddress());
                       GameThread5 gameThread = new GameThread5(socket,hm,Room,Roomnumber,userindex,userdata);
                       System.out.println("3, thread ");
                       gameThread.start();
                       System.out.println("4, thread start ");
                       }catch(Exception e){
                       e.printStackTrace();
                       }
                   }

      }catch(Exception e){

      }

    }
  }

      class GameThread5 extends Thread{
        private Socket sock;
        private ServerSocket serversock;
        private InputStream is;
        private OutputStream os;
        private HashMap<OutputStream,OutputStream> hm ;
        private HashMap<Integer,List<OutputStream>> Room;
        private HashMap<OutputStream,Integer> Roomnumber;
        private HashMap<OutputStream,Integer> userindex;
        private HashMap<OutputStream,byte[]> userdata;
        public GameThread5 (Socket sock, HashMap<OutputStream,OutputStream>  hm, HashMap<Integer,List<OutputStream>> Room,
        HashMap<OutputStream,Integer> Roomnumber,HashMap<OutputStream,Integer> userindex,HashMap<OutputStream,byte[]> userdata){
             this.sock = sock;
             this.hm = hm;
             this.Room = Room;
             this.Roomnumber=Roomnumber;
             this.userindex=userindex;
             this.userdata=userdata;
           try{
             this.is = sock.getInputStream();
             this.os = sock.getOutputStream();


             //전체 그룹에 채널 추가
              synchronized(hm){
                hm.put(os,os);
              }
              //전체가 짝수이면 방을 찾아서 지금 들어온 채널을 2p로 만든다
              System.out.println("hmsize"+hm.size());
              if(hm.size()%2 == 0){

               for(int i=1;i<10000;i++){
                 try{
                 if(Room.get(i).size() == 1){
                   System.out.println("roomnumber:"+i);
                   System.out.println(Room.get(i).size());
                   List<OutputStream> userchannel = new ArrayList<OutputStream>();
                   userchannel = Room.get(i);

                   synchronized(userchannel){
                    userchannel.add(1,os);
                   }
                   synchronized(Room){
                    Room.put(i,userchannel);
                   }
                   synchronized(Roomnumber){
                    Roomnumber.put(os,i);
                   }
                   synchronized(userindex){
                    userindex.put(os,1);
                   }

                 }else if(Room.get(i).size() == 2){

                 }else{

                 }
                 }catch(Exception e){

                 }
               }


              }else{
                 List<OutputStream> userchannel = new ArrayList<OutputStream>();
                 synchronized(userchannel){
                  userchannel.add(0,os);
                 }
                synchronized(Room){
                 Room.put(JavaSocket5.count,userchannel);
                 System.out.println(JavaSocket5.count);
                }
                 synchronized(Roomnumber){
                  Roomnumber.put(os,JavaSocket5.count);
                   System.out.println(JavaSocket5.count);
                 }
                 synchronized(userindex){
                  userindex.put(os,0);
                 }
                 JavaSocket5.count += 1;
                 System.out.println(JavaSocket5.count);
              }

             int nRead;
             int offset;
             int length = 1024;
             byte[] receivedBytes = new byte[length];
            // nRead  = is.read(receivedBytes,0,length);
             System.out.println("length1: "+length);
             is.read(receivedBytes, 0, length);
             ByteBuffer readMessage2 =  ByteBuffer.wrap(receivedBytes);

             Game game = new Game();
             game = Game.getRootAsGame(readMessage2);
            // Player player = new Player();
            // player = Player.getRootAsPlayer(readMessage2);
             Player player = (Player)game.player();

            // System.out.println(player.startstate());
            // byte type = player.startstate();

             FlatBufferBuilder builder;
             byte[] sendBuffer;
             ByteBuffer sendMessage;
             //내 userindex 확인해서 0, 1 확인
             //0 이면 자기 자신한테 1번의 데이터 전달

             System.out.println("userindex: "+userindex.get(os));
             if(userindex.get(os)==0){

                 builder = new FlatBufferBuilder(1024);
                 int idoffset = builder.createString(player.id());
                 int nicknameoffset = builder.createString(player.nickname());
                 Player.startPlayer(builder);
                 Player.addStartstate(builder, player.startstate());
                 Player.addUserindex(builder, player.userindex());
                 Player.addId(builder, idoffset);
                 Player.addNickname(builder, nicknameoffset);
                 Player.addLevel(builder, player.level());
                 Player.addPower(builder, player.power());
                 Player.addHp(builder, player.hp());
                 Player.addRoomnumber(builder, Roomnumber.get(os));
                 Player.addRoomindex(builder, userindex.get(os));
                 Player.addGunid(builder, player.gunid());
                 Player.addArmorid(builder, player.armorid());
                 Player.addSkill1id(builder, player.skill1id());
                 Player.addSkill2id(builder, player.skill2id());

                 int player2 = Player.endPlayer(builder);

                 Game.startGame(builder);
                 Game.addPlayer(builder, player2);
                 Game.addTablenum(builder,0);
                 int game2 = Game.endGame(builder);

                 builder.finish(game2);
                 sendBuffer = builder.sizedByteArray();
                 sendMessage = ByteBuffer.wrap(sendBuffer);

                synchronized (userdata){
                userdata.put(os,sendBuffer);
                }
              //  System.out.println("mydata"+sendMessage);

            }else if(userindex.get(os)==1){
              builder = new FlatBufferBuilder(1024);
              int idoffset = builder.createString(player.id());
              int nicknameoffset = builder.createString(player.nickname());
              Player.startPlayer(builder);
              Player.addStartstate(builder, player.startstate());
              Player.addUserindex(builder, player.userindex());
              Player.addId(builder, idoffset);
              Player.addNickname(builder, nicknameoffset);
              Player.addLevel(builder, player.level());
              Player.addPower(builder, player.power());
              Player.addHp(builder, player.hp());
              Player.addRoomnumber(builder, Roomnumber.get(os));
              Player.addRoomindex(builder, userindex.get(os));
              Player.addGunid(builder, player.gunid());
              Player.addArmorid(builder, player.armorid());
              Player.addSkill1id(builder, player.skill1id());
              Player.addSkill2id(builder, player.skill2id());

              int player2 = Player.endPlayer(builder);
          //    builder.finish(player2);
          //    sendBuffer = builder.sizedByteArray();
          //    sendMessage = ByteBuffer.wrap(sendBuffer);
          Game.startGame(builder);
          Game.addPlayer(builder, player2);
          Game.addTablenum(builder,0);
          int game2 = Game.endGame(builder);

          builder.finish(game2);
          sendBuffer = builder.sizedByteArray();
          sendMessage = ByteBuffer.wrap(sendBuffer);


              synchronized (userdata){
               userdata.put(os,sendBuffer);
              }

              OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(0);

              byte[] enemydata = userdata.get(enemychannel);
            //     System.out.println("enemydata"+enemydata);
              enemychannel.write(sendBuffer, 0, sendBuffer.length);
              System.out.println("length2: "+length);
              os.write(enemydata,0,enemydata.length);
              os.flush();
             }

           }catch(Exception e){

           }


        }public void run(){
          byte[] received=null;

              try{
              while(true){
                int nRead;
                int offset;
                int length = 1024;
                byte[] receivedBytes = new byte[length];
               // nRead  = is.read(receivedBytes,0,length);

                is.read(receivedBytes, 0, length);
                  System.out.println("errerpos1:");

                  if(userindex.get(os)==0){
                  System.out.println("errerpos2:"+ sock.isClosed());
                   OutputStream enemychannel;

                  if(Room.get(Roomnumber.get(os)).size()==1){

                   os.write(1);

                  }else{
                      enemychannel = Room.get(Roomnumber.get(os)).get(1);
                      System.out.println("errerpos3");
                      enemychannel.write(receivedBytes, 0, length);
                      System.out.println("errerpos4");
                      enemychannel.flush();
                      System.out.println("errerpos5");
                  }

               }else if(userindex.get(os)==1){
                 System.out.println("errerpos2:"+ os);
                 OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(0);
                   System.out.println("errerpos3");
                 enemychannel.write(receivedBytes, 0, length);
                   System.out.println("errerpos4");
                 enemychannel.flush();
                   System.out.println("errerpos5");
               }

               System.out.println("errerpos6");
             }

              }catch(SocketException e){

                if(userindex.get(os)==0){
                //  System.out.println("disconnet0-0-1:"+hm.get(os));
                 System.out.println("disconnet0-0-1:"+hm.size());
                  hm.remove(os);
                  System.out.println("disconnet0-0-2:"+hm.size());
                  System.out.println("disconnet0-1-1:"+Room.get(Roomnumber.get(os)));
                  Room.remove(Roomnumber.get(os));

                  System.out.println("disconnet0-1:"+Roomnumber.get(os));
                  Roomnumber.remove(os);
                  System.out.println("disconnet0-2:"+userindex.get(os));
                  userindex.remove(os);
                  System.out.println("disconnet0-3:"+userdata.get(os));
                  userdata.remove(os);
                  System.out.println("disconnet0-4:");
                  try{
                    sock.close();
                  }catch(Exception e2){

                  }


              }else if(userindex.get(os)==1){
            //    System.out.println("disconnet1-0-1:"+hm.get(os));
                System.out.println("disconnet0-0-1:"+hm.size());
                hm.remove(os);
                System.out.println("disconnet1-0-2:"+hm.size());
                System.out.println("disconnet1-1-1:"+Roomnumber.get(os));
                Roomnumber.remove(os);

                System.out.println("disconnet1-1-2:"+userindex.get(os));
                userindex.remove(os);

                System.out.println("disconnet1-3:"+userdata.get(os));
                userdata.remove(os);

                try{
                  sock.close();
                }catch(Exception e2){

                }
              }

            }catch(NullPointerException e3){
             System.out.println(e3);

             if(userindex.get(os)==0){
             //  System.out.println("disconnet0-0-1:"+hm.get(os));
              System.out.println("disconnet0-0-1:"+hm.size());
               hm.remove(os);
               System.out.println("disconnet0-0-2:"+hm.size());
               System.out.println("disconnet0-1-1:"+Room.get(Roomnumber.get(os)));
               Room.remove(Roomnumber.get(os));

               System.out.println("disconnet0-1:"+Roomnumber.get(os));
               Roomnumber.remove(os);
               System.out.println("disconnet0-2:"+userindex.get(os));
               userindex.remove(os);
               System.out.println("disconnet0-3:"+userdata.get(os));
               userdata.remove(os);
               System.out.println("disconnet0-4:");
               try{
                 sock.close();
               }catch(Exception e2){

               }


           }else if(userindex.get(os)==1){
           //    System.out.println("disconnet1-0-1:"+hm.get(os));
             System.out.println("disconnet0-0-1:"+hm.size());
             hm.remove(os);
             System.out.println("disconnet1-0-2:"+hm.size());
             System.out.println("disconnet1-1-1:"+Roomnumber.get(os));
             Roomnumber.remove(os);

             System.out.println("disconnet1-1-2:"+userindex.get(os));
             userindex.remove(os);

             System.out.println("disconnet1-3:"+userdata.get(os));
             userdata.remove(os);

             try{
               sock.close();
             }catch(Exception e2){

             }
           }

         }catch(Exception e5){
           System.out.println(e5);
         }




              }

          }
