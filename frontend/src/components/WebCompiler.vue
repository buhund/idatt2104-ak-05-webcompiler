<script setup lang="ts">
import { ref } from 'vue'

const inputCode = ref('') // To store the input from the user
const outputCode = ref('') // To store the output after compilation

// Example function to simulate code compilation
// This should be replaced with your actual compilation logic
const compileCode = async () => {
  try {
    const response = await fetch('http://localhost:5240/api/compilation/compile', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ code: inputCode.value })
    });
    if (!response.ok) {
      throw new Error(`Error: ${response.statusText}`);
    }
    const result = await response.text();
    outputCode.value = result;
  } catch (error) {
    console.error("Failed to compile code:", error);
    outputCode.value = error.toString();
  }
}

</script>


<template>
  <div class="compiler-container">
    <h1>Sanctus Codixis Compilatoris</h1>

    <!-- Code Input -->
    <textarea v-model="inputCode" placeholder="Type your code here..." rows="10" cols="50"></textarea>

    <!-- Compile Button -->
    <button @click="compileCode">Compile Code</button>

    <!-- Compiled Code Output -->
    <textarea v-model="outputCode" readonly rows="10" cols="50" class="output-area"></textarea>
  </div>
</template>


<style scoped>
.compiler-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
  align-items: center;
  margin-top: 20px;
}

textarea {
  width: 80%;
  padding: 10px;
  font-family: monospace;
  resize: vertical;
}

.output-area {
  background-color: #213547;
  border: 1px solid #ccc;
}
</style>

