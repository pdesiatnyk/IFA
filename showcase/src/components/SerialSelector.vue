<script setup lang="ts">
import { SAMPLE_UDIS } from '../data/sampleUdis.js';

const props = defineProps<{ modelValue: string }>();
const emit = defineEmits<{ 'update:modelValue': [value: string] }>();

const categories = Array.from(new Set(SAMPLE_UDIS.map((s) => s.category)));

function onSelect(event: Event) {
  const value = (event.target as HTMLSelectElement).value;
  if (value) {
    emit('update:modelValue', value);
  }
}

function onTextInput(event: Event) {
  emit('update:modelValue', (event.target as HTMLTextAreaElement).value);
}
</script>

<template>
  <div class="field">
    <label for="sample-select">Sample barcode</label>
    <select id="sample-select" @change="onSelect">
      <option value="">-- pick a sample --</option>
      <optgroup v-for="category in categories" :key="category" :label="category">
        <option
          v-for="sample in SAMPLE_UDIS.filter((s) => s.category === category)"
          :key="sample.label"
          :value="sample.barcode"
        >
          {{ sample.label }}
        </option>
      </optgroup>
    </select>
  </div>
  <div class="field">
    <label for="barcode-input">Barcode</label>
    <textarea id="barcode-input" rows="2" :value="props.modelValue" @input="onTextInput"></textarea>
  </div>
</template>
