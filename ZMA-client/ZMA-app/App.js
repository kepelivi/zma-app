import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View } from 'react-native';
import { useState, useEffect } from 'react';

export default function App() {
  const [hello, setHello] = useState("");

  const port = "http://localhost:5086"

  async function fetchHello() {
    try {
      const res = await fetch(`${port}/Hello`);
      const data = await res.text();
      return data;
    } catch (error) {
      throw error;
    }
  }

  useEffect(() => {
    fetchHello()
    .then(data => setHello(data));
  }, []);

  return (
    <View style={styles.container}>
      <Text>Hi</Text>
      <Text>{hello}</Text>
      <StatusBar style="auto" />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
