<template>
  <v-container fluid class="pa-6">
    <h2 class="font-weight-bold text-center">ادارة الوثائق</h2>

    <v-card elevation="8" class="rounded-xl pa-6 ma-6">
      <div class="d-flex mb-4">
        <v-btn
          class="font-weight-bold"
          color="primary"
          variant="elevated"
          prepend-icon="mdi-plus"
          @click="openDialog"
        >
          اضافة
        </v-btn>
      </div>

      <v-data-table-virtual
        dir="rtl"
        :headers="headers"
        height="400"
        item-value="id"
        :items="personalDataList"
        fixed-header
      >

        <template #item.countDocs="{ item }">
          <v-icon color="black">mdi-file-document-outline</v-icon>
        </template>
        
        <template #item.docs="{ item }">
          <v-icon
            color="secondary"
            style="cursor: pointer"
            @click="openDocsDialog(item)"
          >
            mdi-eye
          </v-icon>
        </template>
        
        <template #item.action="{ item }">
          <v-menu>
            <template #activator="{ props }">
              <v-btn v-bind="props" icon>
                <v-icon>mdi-dots-vertical</v-icon>
              </v-btn>
            </template>
            <v-list>
              <v-list-item @click="openUploadDocsPage(item)">
                <v-list-item-title>
                  <v-icon start color="info">mdi-upload</v-icon>
                  رفع وتعديل الوثائق
                </v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>
        </template>
      </v-data-table-virtual>
    </v-card>
  </v-container>

  <!-- Insert Data -->
  <AddPersonalDataDialog
    v-model="showAddDialog"
    :loading="loading"
    :errorMsg="errorMsg"
    @save="saveDoc"
    @close="closeAddDialog"
  />

  <!-- Show Docs -->
  <UserDocsDialog
    v-model="showDocsDialog"
    :userDocs="selectedUserDocs"
    :userName="selectedUserName"
  />
</template>

<script lang="ts" setup>
import AddPersonalDataDialog from "../components/AddPersonalDataDialog.vue";
import UserDocsDialog from "../components/UserDocsDialog.vue";

import { useRouter } from "vue-router";
import { ref, onMounted } from "vue";

import {
  getAllPersonalData,
  addPersonalData,
  PersonalData,
} from "../api/personalDataService";

import { getUserFiles } from "../api/folderManagerService";
import { validateNumber } from "vuetify/lib/labs/VCalendar/util/timestamp.mjs";

const headers = [
  { title: "التسلسل", align: "start", key: "id" },
  { title: " الاسم الرباعي", align: "start", key: "fullName" },
  { title: "اسم الام الرباعي", align: "start", key: "motherName" },
  { title: "المواليد", align: "birthDate" },
  { title: "الرقم التقاعدي ", align: "start", key: "retirementNumber" },
  { title: "مسلمة الى", align: "start", key: "deliveredTo" },
  { title: "مستلمة من", align: "start", key: "ReceivedFrom" },
  { title: "عدد الوثائق", align: "start", key: "countDocs" },
  { title: "الوثائق", align: "start", key: "docs" },
  { title: " الاجراء", align: "start", key: "action" },
];

const router = useRouter();
function openUploadDocsPage(item) {
  router.push({ name: "folderManager", params: { id: item.id } });
}

// ---------------------------------
const showAddDialog = ref(false);
const showDocsDialog = ref(false);

const dialog = ref(false);

const newDoc = ref({
  fullName: "",
  motherName: "",
  birthDate: "",
  retirementNumber: "",
  deliveredTo: "",
  receivedFrom: "",
});

function openDialog() {
  // dialog.value = true;
  showAddDialog.value = true;
}

function closeAddDialog() {
  showAddDialog.value = false;
}

function closeDialog() {
  dialog.value = false;

  newDoc.value = {
    fullName: "",
    motherName: "",
    birthDate: "",
    retirementNumber: "",
    deliveredTo: "",
    receivedFrom: "",
  };
}

async function saveDoc() {
  // try {
  //   loading.value = true;
  //   await addPersonalData({
  //     fullName: newDoc.value.fullName,
  //     motherName: newDoc.value.motherName,
  //     birthDate: newDoc.value.birthDate,
  //     retirementNumber: Number(newDoc.value.retirementNumber),
  //     deliveredTo: newDoc.value.deliveredTo,
  //     receivedFrom: newDoc.value.receivedFrom,
  //   });
  //   await fetchPersonalData();
  //   closeDialog();
  // } catch (error) {
  //   errorMsg.value = "خطأ في إضافة البيانات";
  // } finally {
  //   loading.value = false;
  // }
  try {
    loading.value = true;
    await addPersonalData({
      fullName: docData.fullName,
      motherName: docData.motherName,
      birthDate: docData.birthDate,
      retirementNumber: Number(docData.retirementNumber),
      deliveredTo: docData.deliveredTo,
      receivedFrom: docData.receivedFrom,
    });
    await fetchPersonalData();
    showAddDialog.value = false;
  } catch (error) {
    errorMsg.value = "خطأ في إضافة البيانات";
  } finally {
    loading.value = false;
  }
}

// ---------------------------------

const personalDataList = ref<PersonalData[]>([]);
const loading = ref(false);
const errorMsg = ref("");

// Add Data
onMounted(async () => {
  await fetchPersonalData();
});
async function fetchPersonalData() {
  loading.value = true;
  try {
    const response = await getAllPersonalData();
    personalDataList.value = response.data.data.data ?? [];
  } catch (error) {
    errorMsg.value = "خطأ في تحميل البيانات";
  } finally {
    loading.value = false;
  }
}

// Dialog Show Docs
const docsDialog = ref(false);
const selectedUserDocs = ref<any[]>([]);
const selectedUserName = ref("");

async function openDocsDialog(item) {
  // try {
  //   loading.value = true;
  //   selectedUserName.value = item.fullName;
  //   const response = await getUserFiles(item.id);
  //   selectedUserDocs.value = response.data.data ?? [];
  //   docsDialog.value = true;
  // } catch (err) {
  //   errorMsg.value = "تعذر تحميل وثائق المستخدم";
  // } finally {
  //   loading.value = false;
  // }
  try {
    loading.value = true;
    selectedUserName.value = item.fullName;
    const response = await getUserFiles(item.id);
    selectedUserDocs.value = response.data.data ?? [];
    showDocsDialog.value = true;
  } catch (err) {
    errorMsg.value = "تعذر تحميل وثائق المستخدم";
  } finally {
    loading.value = false;
  }
}

function isImage(fileName) {
  return /\.(jpg|jpeg|png|gif|bmp|webp)$/i.test(fileName);
}

function getImageUrl(path) {
  return `http://localhost:5089${path}`;
}
</script>

<style scoped>
.card-dialog {
  overflow: visible !important;
  position: relative;
}

.close-btn {
  position: absolute;
  top: -15px;
  left: -15px;
  background-color: var(--primary);
}

.close-btn:hover {
  background-color: var(--alarm);
  color: white;
}

.font-weight-bold {
  color: var(--text);
}

.form {
  text-align: right !important;
  direction: rtl !important;
}

.text-field {
  direction: rtl;
}

.docs-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  justify-content: start;
  align-items: flex-start;
}

.doc-item {
  width: 180px;
  text-align: center;
  margin-bottom: 20px;
}

.doc-thumb {
  width: 160px;
  height: 110px;
  object-fit: contain;
  border: 2px solid #eee;
  border-radius: 12px;
  margin-bottom: 8px;
  background: #f8f9fa;
  box-shadow: 0 2px 6px 0 #eee;
}

.doc-name {
  font-size: 15px;
  font-weight: bold;
  color: #1a1a1a;
  word-break: break-word;
}

.doc-date {
  font-size: 13px;
  color: #888;
}

@media (max-width: 600px) {
  .close-btn {
    top: 8px;
    left: 8px;
  }
}
</style>
