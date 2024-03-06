<script setup lang="ts">
import { ref } from 'vue'

const inputCode = ref('') // Store the input from user
const outputCode = ref('') //Store the output after compilation
const exampleCode = `#include<stdio.h>\n\nint main(void) {\n    printf("Hello World\\n");\n    return 0;\n}`
const isLoading = ref(false); // Initial state is not loading


const compileCode = async () => {
  isLoading.value = true; // Start loading
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
    const contentType = response.headers.get("Content-Type");
    if (contentType && contentType.indexOf("application/json") !== -1) {
      const result = await response.json();
      outputCode.value = result.message;
    } else {
      outputCode.value = await response.text();
    }
  } catch (error) {
    console.error("Failed to compile code:", error);
    outputCode.value = "Compilation failed. Please try again.";
  } finally {
    isLoading.value = false; // Stop loading regardless of the result
  }
}

</script>


<template>
  <div class="compiler-container">
    <h1>Compiler of Code</h1>

    <!-- Code Input -->
    <textarea v-model="inputCode" placeholder="Type your code here..." rows="10" cols="50"></textarea>

    <!-- Compile Button -->
    <button @click="compileCode" :disabled="isLoading">Compile Code</button>
    <div v-if="isLoading">Compiling...</div>
    <!-- TODO: Add some fancy loading spinny schnizzle ma hizzle -->


    <!-- Compiled Code Output -->
    <textarea v-model="outputCode" readonly rows="10" cols="50" class="output-area"></textarea>

    <div class="example-code">
      <p>Example code:</p>
      <textarea readonly :value="exampleCode" class="example"></textarea>
    </div>
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


 .example-code {
   margin-top: 20px;
 }

.example {
  width: 300px;
  height: 140px;
  font-family: monospace;
  overflow: auto;
  white-space: pre;
}
</style>

