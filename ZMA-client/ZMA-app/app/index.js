import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View } from 'react-native';

//constants
import {COLORS} from '../constants/theme';

//components
import Login from '../components/login';
import Logo from '../components/logo';
import Camera from '../components/camera';

export default function Home() {
  return (
    <View style={styles.container}>
      <Logo />
      <StatusBar style="auto" />
      <Login />
      <Text style={styles.main}>Vendég vagy?</Text>
      <Text style={styles.text}>Scanneld be a QR kódot!</Text>
      <Camera />
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
    fontSize: 20,
    color: COLORS.black,
    fontWeight: '500',
  },
  text: {
    fontSize: 17,
    color: COLORS.greyish
  }
});