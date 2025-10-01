<script setup>
import axios from 'axios'
import { ref } from "vue"

const isAuth = ref(false)
const log = ref("")
const password = ref("")
const currentUser = ref(null)
const users = ref([])
const errorMessage = ref("")

const apiLogin = async (login, password) => {
  const response = await axios.post('http://localhost:3000/api/Users', {
    Login: login,
    Password: password
  })
  return response.data
}

const apiGetUsers = async () => {
  const response = await axios.get('http://localhost:3000/api/Users')
  return response.data
}


const login = async () => {
  try {
    errorMessage.value = ""

    const userData = await apiLogin(log.value, password.value)
    currentUser.value = userData
    isAuth.value = true

    if (currentUser.value.idRole === 1) {
      users.value = await apiGetUsers()
    }
  } catch (error) {
    errorMessage.value = "Ошибка авторизации"
  }
}

const logout = () => {
  isAuth.value = false
  currentUser.value = null
  users.value = []
  log.value = ""
  password.value = ""
  errorMessage.value = ""
}
</script>

<template>
  <div class="container">
    <header>
      <h3>Докер Веб-клиент</h3>
      <div v-if="isAuth" class="user-info">
        <span>{{ currentUser?.login }}</span>
        <button @click="logout">Выйти</button>
      </div>
    </header>
    <main>
      <div v-if="!isAuth" class="auth">
        <h4>Авторизация</h4>
        <input v-model="log" placeholder="Логин" />
        <input v-model="password" type="password" placeholder="Пароль" />
        <button @click="login">Войти</button>
        <div v-if="errorMessage" class="error">{{ errorMessage }}</div>
      </div>
      <div v-else>
        <div v-if="currentUser.idRole === 1">
          <h4>Пользователи</h4>
          <table>
            <thead>
            <tr>
              <th>ID</th>
              <th>Логин</th>
              <th>Роль</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="user in users" :key="user.id">
              <td>{{ user.idUser }}</td>
              <td>{{ user.login }}</td>
              <td>{{ user.idRole === 1 ? 'Админ' : 'Клиент' }}</td>
            </tr>
            </tbody>
          </table>
        </div>
        <div v-else>
          <h4>Ваши данные</h4>
          <div class="user-data">
            <div><strong>ID:</strong><strong> {{ currentUser.idUser }}</strong></div>
            <div><strong>Логин:</strong> <strong>{{ currentUser.login }}</strong></div>
            <div><strong>Роль:</strong><strong> Клиент</strong></div>
            <div v-if="currentUser.name"><strong>Name:</strong> <strong>{{ currentUser.name }}</strong></div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<style scoped>

h3{
  color: black;
}
h4{
  color: black;
}
th{
  color: black;
}
td{
  color: black;
}
strong {
  color: black;
}
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
  padding-bottom: 10px;
  border-bottom: 1px solid #ddd;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 10px;
}

.auth {
  max-width: 300px;
  margin: 0 auto;
}

.auth h4 {
  margin-bottom: 15px;
}

input {
  width: 100%;
  padding: 8px;
  margin-bottom: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

button {
  padding: 8px 16px;
  background: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background: #0056b3;
}

.error {
  color: red;
  margin-top: 10px;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}

th, td {
  padding: 8px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background: #f5f5f5;
}

.user-data div {
  margin-bottom: 8px;
  padding: 5px 0;
}
</style>