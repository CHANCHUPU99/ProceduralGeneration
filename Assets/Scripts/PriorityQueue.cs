using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private List<(T item, float priority)> elements = new List<(T, float)>();
    public int Count => elements.Count;
    //funcion to add to the queue
    public void enqueue(T item, float priority) {
        elements.Add((item, priority));
    }
    //funcion to remove
    public T dequeue() {
        int bestIndex = 0;
        for(int i =1; i < elements.Count; i++) {
            if(elements[i].priority < elements[bestIndex].priority) {
                bestIndex = i;
            }
        }
        T bestItem = elements[bestIndex].item;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}
