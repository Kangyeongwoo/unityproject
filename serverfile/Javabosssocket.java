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
import Bossdata.Game;
import Bossdata.Item;
import Bossdata.Player;
import Bossdata.PlayerStart;
import Bossdata.MoveState;
import Bossdata.Vec3;
import Bossdata.Monster;
import java.net.SocketTimeoutException;
import java.lang.NullPointerException;
public class Javabosssocket {

    static int count = 1;
  //  static int bosshp = 1000;

    public static void main(String[] args){
     try{
       //소켓 생성 4343포트 입력 10개까지 (queue)

       ServerSocket  serverSocket = new ServerSocket(4345, 10);
       System.out.println("Socket time: "+serverSocket.getSoTimeout());
       HashMap<OutputStream,OutputStream> hm= new HashMap<OutputStream,OutputStream>();
       HashMap<Integer,List<OutputStream>> Room = new HashMap<Integer,List<OutputStream>>();
       HashMap<OutputStream,Integer> Roomnumber = new HashMap<OutputStream,Integer>();
       HashMap<OutputStream,Integer> userindex = new HashMap<OutputStream,Integer>();
       HashMap<OutputStream,byte[]> userdata = new HashMap<OutputStream,byte[]>();
       HashMap<Integer,Integer> roombosshp = new HashMap<Integer,Integer>();

       while(true) {
                       //
                       Socket socket = serverSocket.accept();

                       try{
                       System.out.println("1, wait ");
                       System.out.println("2, connet :"+socket.getRemoteSocketAddress());
                       BossThread bossThread = new BossThread(socket,hm,Room,Roomnumber,userindex,userdata,roombosshp );
                       System.out.println("3, thread ");
                       bossThread.start();
                       System.out.println("4, thread start ");
                       }catch(Exception e){
                       e.printStackTrace();
                       }
                   }

      }catch(Exception e){

      }

    }
  }


  class BossThread extends Thread{
    private Socket sock;
    private ServerSocket serversock;
    private InputStream is;
    private OutputStream os;
    private HashMap<OutputStream,OutputStream> hm ;
    private HashMap<Integer,List<OutputStream>> Room;
    private HashMap<OutputStream,Integer> Roomnumber;
    private HashMap<OutputStream,Integer> userindex;
    private HashMap<OutputStream,byte[]> userdata;
    private HashMap<Integer,Integer> roombosshp ;
    public int bosshp;


    public BossThread (Socket sock, HashMap<OutputStream,OutputStream>  hm, HashMap<Integer,List<OutputStream>> Room,
    HashMap<OutputStream,Integer> Roomnumber,HashMap<OutputStream,Integer> userindex,HashMap<OutputStream,byte[]> userdata , HashMap<Integer,Integer> roombosshp){
         this.sock = sock;
         this.hm = hm;
         this.Room = Room;
         this.Roomnumber=Roomnumber;
         this.userindex=userindex;
         this.userdata=userdata;
         this.roombosshp=roombosshp;
          try{
         this.is = sock.getInputStream();
         this.os = sock.getOutputStream();
             System.out.println("oslog:"+os);
         //전체 그룹에 채널 추가
          synchronized(hm){
            hm.put(os,os);
          }
          System.out.println("roomisempty:"+Room.isEmpty());
        if(Room.isEmpty()){
          //방 만들기
          List<OutputStream> userchannel = new ArrayList<OutputStream>();
          synchronized(userchannel){
           userchannel.add(0,os);
          }
         synchronized(Room){
          Room.put(Javabosssocket.count,userchannel);
          System.out.println(Javabosssocket.count);
         }
         synchronized(roombosshp){
          roombosshp.put(Javabosssocket.count,2000);
          System.out.println(Javabosssocket.count);
         }
          synchronized(Roomnumber){
           Roomnumber.put(os,Javabosssocket.count);
            System.out.println(Javabosssocket.count);
          }
          synchronized(userindex){
           userindex.put(os,0);
          }
          Javabosssocket.count += 1;
          System.out.println(Javabosssocket.count);


        }else{
          //방 확인 하고 비어 있는 방 있으면 들어가고 없으면 방 만들기
          for( Integer key : Room.keySet() ){
             System.out.println("key:"+key);
             System.out.println("roomsize:"+Room.get(key).size());

           if(Room.get(key).size()!=3){
             System.out.println("roomsize:"+Room.get(key).size());
             // 방안에 들어있는 플레이어 리스트
             List<OutputStream> userchannel = new ArrayList<OutputStream>();
             userchannel = Room.get(key);

             //없던거
             List<Integer> userindexlist = new ArrayList<Integer>();
             for(int j=0; j<Room.get(key).size() ; j++){
               //Room.get(key).get(i) == user들의 os 이 os 가 가지는 인덱스를 확인해서 적절한 값을 넣어줘야된다.
               synchronized(userindexlist){
                userindexlist.add(userindex.get(Room.get(key).get(j)));
               }

             }

             for(int k=0; k<3 ; k++){
              if(!userindexlist.contains(k)){
                synchronized(userindex){
                 userindex.put(os,k);
                // userindex.put(os,Room.get(key).size());
                 System.out.println("roomsize2:"+userindex.get(os));
                 break;
                }

              }
             }

            /*
             synchronized(userindex){

              userindex.put(os,Room.get(key).size());
              System.out.println("roomsize2:"+userindex.get(os));
             }
             */

             synchronized(userchannel){
              //userchannel.add(Room.get(key).size(),os);
              userchannel.add(os);
             }

             synchronized(Room){
              Room.put(key,userchannel);
             }
             synchronized(Roomnumber){
              Roomnumber.put(os,key);
             }

             break;
           }else{
               System.out.println("allroom3:");
             //모든 방이 3인 일때 방 만들어야됨
             List<OutputStream> userchannel = new ArrayList<OutputStream>();
             synchronized(userchannel){
              userchannel.add(0,os);
             }
            synchronized(Room){
             Room.put(Javabosssocket.count,userchannel);
             System.out.println(Javabosssocket.count);
            }
            synchronized(roombosshp){
             roombosshp.put(Javabosssocket.count,2000);
             System.out.println(Javabosssocket.count);
            }
             synchronized(Roomnumber){
              Roomnumber.put(os,Javabosssocket.count);
               System.out.println(Javabosssocket.count);
             }
             synchronized(userindex){
              userindex.put(os,0);
             }
             Javabosssocket.count += 1;
             System.out.println(Javabosssocket.count);
           }

          }

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


        System.out.println("userindex: "+userindex.get(os));

        if(Room.get(Roomnumber.get(os)).size()==3){
          for(int i=0; i<3; i++){
            //인덱스 0번 데이터를 0번 1번 2번 한테 보내
           OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(i);
           byte[] enemydata = userdata.get(enemychannel);

           for(int j=0; j<3; j++){

             OutputStream enemychannel2 = Room.get(Roomnumber.get(os)).get(j);
             enemychannel2.write(enemydata, 0, enemydata.length);
             System.out.println("length2: "+length);
             enemychannel2.flush();
           }
           Thread.sleep(500);

          }
        }


        /*
        if(userindex.get(os)==0){
           //userindex가 0일때
           os.write(sendBuffer, 0, sendBuffer.length);
           os.flush();

         //  System.out.println("mydata"+sendMessage);

       }else if(userindex.get(os)==1){

        os.write(sendBuffer, 0, sendBuffer.length);
        os.flush();
        Thread.sleep(200);
        for(int i=0; i<1; i++){
         OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(i);

         byte[] enemydata = userdata.get(enemychannel);

         enemychannel.write(sendBuffer, 0, sendBuffer.length);
         System.out.println("length2: "+length);
         os.write(enemydata,0,enemydata.length);
         os.flush();
        }


       }else if(userindex.get(os)==2){

         os.write(sendBuffer, 0, sendBuffer.length);
         os.flush();
           Thread.sleep(200);
         for(int i=0; i<2; i++){
          OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(i);
          byte[] enemydata = userdata.get(enemychannel);


          System.out.println("length2: "+length);
          os.write(enemydata,0,enemydata.length);

          enemychannel.write(sendBuffer, 0, sendBuffer.length);
          os.flush();
          Thread.sleep(200);
         }


         }
          */
       }catch(Exception e){
          System.out.println(e);
       }



    }
    public void run(){
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
                 String str = new String(receivedBytes,"UTF-8");
                 str = str.trim();
                 System.out.println("str1:"+str);
                 System.out.println("str1-1:"+str.equals("close"));
             if(str.equals("close")){

               System.out.println("disconnet0-0-1:"+hm.size());
                hm.remove(os);
                System.out.println("disconnet0-0-2:"+hm.size());
                System.out.println("disconnet0-1-1:"+Room.get(Roomnumber.get(os)));

                if(Room.get(Roomnumber.get(os)).size()==1){
                  Room.remove(Roomnumber.get(os));
                  roombosshp.remove(Roomnumber.get(os));
                }else{

                }

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
                   System.out.println("e2:"+e2);
                }
                break;
             }

             /*
             if(receivedBytes == null){
                System.out.println("receivedBytes:null");
                String str = new String(receivedBytes,"UTF-8");
                System.out.println("str:"+str);
             }else{

                String str = new String(receivedBytes);
                str = str.trim();
                if(str.isEmpty()){
                    System.out.println("str1:"+str);

                    System.out.println("disconnet0-0-1:"+hm.size());
                     hm.remove(os);
                     System.out.println("disconnet0-0-2:"+hm.size());
                     System.out.println("disconnet0-1-1:"+Room.get(Roomnumber.get(os)));

                     if(Room.get(Roomnumber.get(os)).size()==1){
                       Room.remove(Roomnumber.get(os));
                       roombosshp.remove(Roomnumber.get(os));
                     }else{

                     }

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
                       break;
                     }

                    break;


                }else{
                    System.out.println("str2:"+str);
                }


             }
             */


              // os 가 있는 roomnumber를 찾는다
              // 그 방안에 모든 os 들에게 데이터를 보낸다 나빼고


              ByteBuffer readMessage =  ByteBuffer.wrap(receivedBytes);
               System.out.println("number1");
              Game game3 = new Game();
               System.out.println("number2");
              game3 = Game.getRootAsGame(readMessage);
               System.out.println("number3");
             // Player player = new Player();
             // player = Player.getRootAsPlayer(readMessage2);
              if(game3.tablenum()==1){
                System.out.println("test1");
                Monster monster = (Monster)game3.monster(0);
                Player playerpow = (Player)game3.player();

                System.out.println("testatk"+monster.attacked());
                if(monster.attacked()==1){
                         System.out.println("test2");
                         int bosshp = roombosshp.get(Roomnumber.get(os));
                         bosshp -= playerpow.power();
                         roombosshp.put(Roomnumber.get(os),bosshp);

                         System.out.println("lasthp:"+bosshp);
                         FlatBufferBuilder builder;
                         byte[] sendBuffer;
                         ByteBuffer sendMessage;

                         builder = new FlatBufferBuilder(1024);

                         Monster.startMonster(builder);
                         Monster.addAttacked(builder,1);
                         Monster.addCurrenthp(builder,bosshp);
                         int monster2 = Monster.endMonster(builder);

                         int[] monsterarray = new int[1];
                         monsterarray[0] = monster2;
                         int monsteroffset = Game.createMonsterVector(builder,monsterarray);

                         Game.startGame(builder);
                         Game.addMonster(builder,monsteroffset );
                         Game.addTablenum(builder,1);
                         int game2 = Game.endGame(builder);
                         builder.finish(game2);
                         sendBuffer = builder.sizedByteArray();

                         for( int i=0; i<3; i++){

                               OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(i);
                               enemychannel.write(sendBuffer, 0, sendBuffer.length);
                               enemychannel.flush();

                         }


                }else{

                  for( int i=0; i<3; i++){

                    if(userindex.get(os)!=i){
                        OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(i);
                        enemychannel.write(receivedBytes, 0, length);
                        enemychannel.flush();
                    }else{
                      //같은 놈 한테는 안보낸다
                    }

                  }
                }



              }else{
                System.out.println("test2");
                for( int i=0; i<3; i++){

                  if(userindex.get(os)!=i){
                      OutputStream enemychannel = Room.get(Roomnumber.get(os)).get(i);
                      System.out.println("enemychannel:"+enemychannel);
                      enemychannel.write(receivedBytes, 0, length);
                      enemychannel.flush();
                  }else{
                    //같은 놈 한테는 안보낸다
                  }

                }

              }

           }

          }catch(SocketException e){
              System.out.println("SocketException:"+e);
          //  if(userindex.get(os)==0){
            //  System.out.println("disconnet0-0-1:"+hm.get(os));
             System.out.println("disconnet0-0-1:"+os);
              hm.remove(os);
              System.out.println("disconnet0-0-2:"+hm.size());
              System.out.println("disconnet0-1-1:"+Room.get(Roomnumber.get(os)));

              if(Room.get(Roomnumber.get(os)).size()==1){
                Room.remove(Roomnumber.get(os));
                roombosshp.remove(Roomnumber.get(os));
              }else{

              }

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
                 System.out.println("e2:"+e2);
              }


          //}
          /*
          else if(userindex.get(os)==1||userindex.get(os)==2){
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
          */
     }catch(Exception e3){
        System.out.println("SocketException:"+e3);
       System.out.println("disconnet0-0-1:"+hm.size());
        hm.remove(os);
        System.out.println("disconnet0-0-2:"+hm.size());
        System.out.println("disconnet0-1-1:"+Room.get(Roomnumber.get(os)));

        if(Room.get(Roomnumber.get(os)).size()==1){
          Room.remove(Roomnumber.get(os));
          roombosshp.remove(Roomnumber.get(os));
        }else{

        }

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
           System.out.println("e2:"+e2);
        }

     }
  }
}
