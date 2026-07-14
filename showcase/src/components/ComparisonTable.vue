<script setup lang="ts">
import { computed, ref } from 'vue';
import { diffRows, describeSection } from '../lib/diff.js';
import { FIELD_META } from '../data/fieldMeta.js';

const props = defineProps<{ left: unknown; right: unknown; leftLabel: string; rightLabel: string }>();

const showOnlyMismatches = ref(false);
const showRawJson = ref(false);

const rows = computed(() => diffRows(props.left, props.right));
const visibleRows = computed(() => (showOnlyMismatches.value ? rows.value.filter((r) => r.mismatch) : rows.value));
</script>

<template>
  <div>
    <div class="field-grid" style="margin-bottom: 0.75rem">
      <label style="display: flex; align-items: center">
        <input v-model="showOnlyMismatches" type="checkbox" style="width: auto; margin-right: 0.35rem" />
        Show only mismatches
      </label>
      <label style="display: flex; align-items: center">
        <input v-model="showRawJson" type="checkbox" style="width: auto; margin-right: 0.35rem" />
        Show raw JSON
      </label>
    </div>

    <div v-if="showRawJson" class="two-col">
      <pre class="output-code">{{ JSON.stringify(left, null, 2) }}</pre>
      <pre class="output-code">{{ JSON.stringify(right, null, 2) }}</pre>
    </div>

    <table v-else class="diff-table">
      <thead>
        <tr>
          <th>Field</th>
          <th>{{ leftLabel }}</th>
          <th>{{ rightLabel }}</th>
          <th>Section</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in visibleRows" :key="row.path" :class="{ mismatch: row.mismatch }">
          <td>
            {{ row.path }}
            <div v-if="FIELD_META[row.path]" style="opacity: 0.65; font-size: 0.8em">{{ FIELD_META[row.path] }}</div>
          </td>
          <td>{{ row.left }}</td>
          <td>{{ row.right }}</td>
          <td style="opacity: 0.65; font-size: 0.8em">{{ describeSection(row.path) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
