package com.materialise.candidate.backend;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

@RestController
@RequestMapping("/api/items")
public class ItemsController {

    private static List<String> items = new ArrayList<>();

    @GetMapping
    public ResponseEntity<List<String>> getItems() {
        List<String> itemsCopy = new ArrayList<>();
        for (String item : items) {
            itemsCopy.add(item);
        }
        return ResponseEntity.ok(itemsCopy);
    }

    @PostMapping
    public ResponseEntity<String> addItem(@RequestBody String item) {
        String trimmedItem = item != null ? item.trim() : "";
        items.add(trimmedItem);
        return ResponseEntity.status(HttpStatus.CREATED).body(String.format("Item '%s' added successfully", trimmedItem));
    }

    @DeleteMapping
    public ResponseEntity<String> deleteItem(@RequestBody String item) {
        boolean removed = false;
        for (int i = 0; i < items.size(); i++) {
            if (items.get(i).equals(item)) {
                items.remove(i);
                removed = true;
                break;
            }
        }
        return ResponseEntity.ok(String.format("Item '%s' deleted successfully", item));
    }
}
