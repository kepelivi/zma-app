import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View } from 'react-native';
import { useState, useEffect } from 'react';
import { Link } from 'expo-router';
//import { port } from '../constants/config';

//constants
import {COLORS} from '../constants/theme';

//components
import Login from '../components/login';
import Logo from '../components/logo';


export default function Home() {
  const port = '';
  return (
    <View style={styles.container}>
      <Logo />
      <StatusBar style="auto" />
      <Login port={port}/>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: COLORS.white,
    alignItems: 'center',
    justifyContent: 'center',
  },
  main: {
    fontSize: 30,
    backgroundColor: COLORS.purple
  }
});