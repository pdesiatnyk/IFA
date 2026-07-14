<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import SerialSelector from './components/SerialSelector.vue';
import ResultSummary from './components/ResultSummary.vue';
import ComparisonTable from './components/ComparisonTable.vue';
import GeneratorTab from './components/GeneratorTab.vue';
import { parseWithTs } from './services/tsParser.js';
import { parseWithCSharp } from './services/csharpApi.js';
import { SAMPLE_UDIS } from './data/sampleUdis.js';
import type { ParseOutcome } from './types.js';

type Tab = 'compare' | 'generate';
const activeTab = ref<Tab>('compare');
const barcode = ref(SAMPLE_UDIS[0].barcode);

const tsOutcome = computed<ParseOutcome>(() => parseWithTs(barcode.value));

const csharpOutcome = ref<ParseOutcome>({ loading: true });
let requestSeq = 0;
watch(
  barcode,
  async (value) => {
    const seq = ++requestSeq;
    csharpOutcome.value = { loading: true };
    try {
      const outcome = await parseWithCSharp(value);
      if (seq === requestSeq) {
        csharpOutcome.value = outcome;
      }
    } catch (err) {
      if (seq === requestSeq) {
        csharpOutcome.value = { success: false, error: { message: String(err) } };
      }
    }
  },
  { immediate: true },
);

function useInCompare(generatedBarcode: string) {
  barcode.value = generatedBarcode;
  activeTab.value = 'compare';
}
</script>

<template>
  <div class="app">
    <header class="app-header">
      <h1>IFA UDI Parser Showcase</h1>
      <p class="subtitle">Cross-language (TypeScript / C#) parse &amp; build demo for IFA UDI barcodes</p>
    </header>

    <div class="tabs" role="tablist">
      <button
        type="button"
        role="tab"
        :aria-selected="activeTab === 'compare'"
        :class="{ active: activeTab === 'compare' }"
        @click="activeTab = 'compare'"
      >
        Compare
      </button>
      <button
        type="button"
        role="tab"
        :aria-selected="activeTab === 'generate'"
        :class="{ active: activeTab === 'generate' }"
        @click="activeTab = 'generate'"
      >
        Generate
      </button>
    </div>

    <main class="tab-panel">
      <div v-if="activeTab === 'compare'">
        <SerialSelector v-model="barcode" />

        <div class="two-col" style="margin: 1rem 0">
          <ResultSummary title="TypeScript" :outcome="tsOutcome" />
          <ResultSummary title="C#" :outcome="csharpOutcome" />
        </div>

        <ComparisonTable :left="tsOutcome" :right="csharpOutcome" leftLabel="TypeScript" rightLabel="C#" />
      </div>

      <GeneratorTab v-else @use-in-compare="useInCompare" />
    </main>
  </div>
</template>
