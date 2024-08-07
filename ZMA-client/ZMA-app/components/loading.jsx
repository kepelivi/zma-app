import React from 'react';
import { View, Text, ActivityIndicator, StyleSheet } from 'react-native';

import { COLORS } from '../constants/theme';

export default function Loading({ message = "Loading..." }) {
    return (
        <View style={styles.container}>
          <ActivityIndicator size="large" color={COLORS.accent} />
          <Text style={styles.text}>{message}</Text>
        </View>
      );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
  },
  text: {
    marginTop: 16,
    fontSize: 18,
    color: COLORS.deepPurple,
    fontWeight: 'bold',
  },
});