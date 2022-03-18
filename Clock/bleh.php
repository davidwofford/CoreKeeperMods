<?php

// random shit saying it's a vending machine
// wait for coin entry
// Check for input
// Validate input against slots and make sure cost is enough
// If not call them dumb and wait for more money
// If yes give them item and change and remove item from inventory
// Validate the slot has something in it


class VendingMachine
{
    public []Slot $slots

    public function init()
    {
        // Set up the slots
        for i = 0; i <= 9; i++
        {
            $items = []Item
            for j = 0; j <= 9; j++ {
                $items[j] = New Item("Item {$j}", (rand(0, 10) / 10));
            }

            $this->slots[i] = New Slot($items)
        }
    }

    public function checkEntry(float $money, int $slotNumber) float
    {
        // Make sure slot exists
        if (!array_key_exists($slotNumber, $this->$slots)) {
            // throw and error and call them dumb
        }

        // Slot exists, make sure cash money good
        if ($this->$slots[$slotNumber]->$items[0]->$cost > $money) {
            // Get your broke ass outta here
        }

        $returnMoney = $money - $this->$slots[$slotNumber]->$items[0]->$cost
        // Vend the item and give back change
        $this->$slots[$slotNumber]->removeItem();

        return $returnMoney
    }
}

class Slot
{
    public []Items $items

    public function __construct([]Items $items)
    {
        $this->$items = $items;
    }

    public function removeItem()
    {
        $this->items = array_shift($this-$items)
    }
}

class Item
{
    public string $name
    public float $cost

    public function __construct(string $name, float $cost)
    {
        $this->$name = $name;
        $this->$cost = $cost;
    }
}

?>