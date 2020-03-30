using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class topvpready : MonoBehaviour
{
   public void pvpready() {

        //전투가 끝나고 ready 신으로 돌아갈때 소켓 다 끊고 시간도 원래대로 돌린다 static 변경 필요
        Findenemy_flat.serverMessage2 = null;

        Findenemy_flat.socketConnection.Close();

        Findenemy_flat.stream = null;

        Control_flat.serverMessgae2 = null;

        Control_flat.receivequeue.Clear();

        Control_flat.clickcount = 0;

        Time.timeScale = 1f;

        StopCoroutine(Control_flat.co_my_coroutine);

        SceneManager.LoadScene("Pvpready");

    }
}
