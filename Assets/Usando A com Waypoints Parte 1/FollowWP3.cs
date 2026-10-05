using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowW3 : MonoBehaviour
{
    Transform goal;

    float speed = 5.0f;
    float accuracy = 5.0f;
    float rotSpeed = 2.0f;

    public GameObject wpManager;

    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;

    Graph g;

    // Controla se o tanque já começou a andar
    bool started = false;

    void Start()
    {
        wps = wpManager.GetComponent<WPManager2>().waypoints;
        g = wpManager.GetComponent<WPManager2>().graph;

        currentNode = wps[4];

        Time.timeScale = 5;
    }

    // Botão: Go to Helipad
    public void GoToHeli()
    {
        started = true;

        g.AStar(currentNode, wps[0]);
        currentWP = 0;
    }

    // Botão: Go to Ruin
    public void GoToRuin()
    {
        started = true;

        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }

    // Botão: Go to Factory
    public void GoToFactory()
    {
        started = true;

        g.AStar(currentNode, wps[4]);
        currentWP = 0;
    }

    void LateUpdate()
    {
        // Se ainda não clicámos em nenhum botão,
        // o tanque fica parado.
        if (!started)
            return;

        // Se não houver caminho, não faz nada.
        if (g.pathList.Count == 0 || currentWP == g.pathList.Count)
            return;

        // Verifica se o tanque chegou ao waypoint atual.
        if (Vector3.Distance(
            g.pathList[currentWP].getId().transform.position,
            this.transform.position) < accuracy)
        {
            currentWP++;
        }

        // Continua a seguir o caminho.
        if (currentWP < g.pathList.Count)
        {
            goal = g.pathList[currentWP].getId().transform;

            Vector3 lookGoal = new Vector3(
                goal.position.x,
                this.transform.position.y,
                goal.position.z
            );

            Vector3 direction = lookGoal - this.transform.position;

            // Faz o tanque virar para o próximo waypoint.
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);

                this.transform.rotation = Quaternion.Slerp(
                    this.transform.rotation,
                    rotation,
                    rotSpeed * Time.deltaTime
                );
            }

            // Faz o tanque andar.
            this.transform.Translate(
                0,
                0,
                speed * Time.deltaTime
            );
        }
    }
}