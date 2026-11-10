using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionQueue : MonoBehaviour
{
    [SerializeField] private float maxWaitTime = 1.5f;
    [SerializeField] private int maxQueueSize = 5;
    [Tooltip("Se vero, le azioni vengono eseguite in sequenza, in base all'ordine in cui sono premute. " +
        "Altrimenti, le prime disponibili vengono eseguite appena è possibile")]
    [SerializeField] private bool waitForFirstActionToComplete = false;
    private List<(Func<bool> CanRun, Action Run)> playerActionQueue = new();


    public void ExecuteAction(Func<bool> canRun, Action run)
    {
        Debug.Log("Enqueueing action: " + run.Method.Name);
        if (playerActionQueue.Count >= maxQueueSize || playerActionQueue.Contains((canRun, run)))
        {
            Debug.Log($"Action: {run.Method.Name} | Queue is full or action already in queue, cannot enqueue action.");
            return;
        }
        playerActionQueue.Add((canRun, run)); // era Enqueue
        StartCoroutine(ExecuteActionCoroutine(canRun, run));
    }

    private IEnumerator ExecuteActionCoroutine(Func<bool> canRun, Action run)
    {
        float _currentWaitTime = 0f;
        Debug.Log("Action added to queue: " + run.Method.Name);

        // Si aspetta finché l'azione non può partire. In modalità sequenza può partire solo se è anche in testa alla lista.
        while (!canRun() || waitForFirstActionToComplete && playerActionQueue[0] != (canRun, run))
        {
            _currentWaitTime += Time.deltaTime;
            if (_currentWaitTime >= maxWaitTime) // il controllo sulla testa non è più qui: aspettare il proprio turno non è un timeout
            {
                Debug.Log($"Action: {run.Method.Name} | Waited too long, stopping action.");
                playerActionQueue.Remove((canRun, run)); // era Dequeue(): toglie questa azione, non la testa
                yield break;
            }
            yield return null;
        }
        run();
        playerActionQueue.Remove((canRun, run)); // idem
    }
}